using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDeployService _deployService;
        private IList<DeployTargetDevice> _devices;
        private IList<WinApp> _installedApps;

        private DeployTargetDevice _selectedDevice;

        public MainViewModel(INavigationService navigationService, IDeployService deployService)
        {
            this._navigationService = navigationService;
            this._deployService = deployService;
            this.CreateCommands();
        }

        public ICommand RefreshDevicesCommand { get; private set; }

        public ICommand SettingsCommand { get; private set; }

        public IList<DeployTargetDevice> Devices
        {
            get { return this._devices; }
            set { this.Set(() => this.Devices, ref this._devices, value); }
        }

        public DeployTargetDevice SelectedDevice
        {
            get { return this._selectedDevice; }
            set
            {
                this.Set(() => this.SelectedDevice, ref this._selectedDevice, value);
                this._navigationService.NavigateTo(PageKey.DeviceDetails, value);
            }
        }

        public ICommand TestCommand { get; private set; }

        public IList<WinApp> InstalledApps
        {
            get { return this._installedApps; }
            set { this.Set(() => this.InstalledApps, ref this._installedApps, value); }
        }

        private void CreateCommands()
        {
            this.RefreshDevicesCommand = new RelayCommand(async () => { await this.RefreshDevicesAsync(); });
          
            //this.SettingsCommand = new RelayCommand(Open Settings page);
            this.TestCommand = new RelayCommand(async () => { await this.LoadAppsAsync(); });
        }

        private async Task LoadAppsAsync()
        {
            if (this.SelectedDevice == null)
            {
                return;
            }

            this.IsBusy = true;
            var apps = await this._deployService.GetInstalledAppsAsync(this.SelectedDevice);
            this.InstalledApps = apps;
            this.IsBusy = false;
        }

      

        public async Task InitializeAsync()
        {
            // TODO: Load data
            await this.RefreshDevicesAsync();
        }

        private async Task RefreshDevicesAsync()
        {
            this.IsBusy = true;
            this.Devices = await this._deployService.GetDevicesAsync();

            this.IsBusy = false;
        }
    }
}