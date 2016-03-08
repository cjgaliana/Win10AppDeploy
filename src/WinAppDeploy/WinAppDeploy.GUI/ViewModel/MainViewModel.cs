using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IDeployService _deployService;

        private bool _isBusy;
        private bool _isCommandInstalled;
        private IList<DeployTargetDevice> _devices;
        private DeployTargetDevice _selectedDevice;
        private IList<WinApp> _installedApps;

        public MainViewModel(IDeployService deployService)
        {
            this._deployService = deployService;
            this.CreateCommands();
        }

        public ICommand RefreshDevicesCommand { get; private set; }
        public ICommand InstallSdkCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }

        public bool IsCommandInstalled
        {
            get { return this._isCommandInstalled; }
            set { this.SetProperty(ref this._isCommandInstalled, value); }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set { this.SetProperty(ref this._isBusy, value); }
        }

        public IList<DeployTargetDevice> Devices
        {
            get { return this._devices; }
            set { this.SetProperty(ref this._devices, value); }
        }

        public DeployTargetDevice SelectedDevice
        {
            get { return this._selectedDevice; }
            set
            {
                this.SetProperty(ref this._selectedDevice, value);
                this.LoadAppsAsync();
            }
        }

        public ICommand TestCommand { get; private set; }

        public IList<WinApp> InstalledApps
        {
            get { return this._installedApps; }
            set { this.SetProperty(ref this._installedApps, value); }
        }

        private void CreateCommands()
        {
            this.RefreshDevicesCommand = new DelegateCommand(async () => { await this.RefreshDevicesAsync(); });
            this.InstallSdkCommand = new DelegateCommand(this.OpenSdkWebSite);
            //this.SettingsCommand = new RelayCommand(Open Settings page);
            this.TestCommand = new DelegateCommand(async () => { await this.LoadAppsAsync(); });
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

        private void OpenSdkWebSite()
        {
            Process.Start("https://dev.windows.com/en-us/downloads/windows-10-sdk");
        }

        public async Task InitializeAsync()
        {
            // TODO: Load data
            this.IsBusy = true;
            this.IsCommandInstalled = await this._deployService.IsInstalledAsync();
            this.IsBusy = false;
        }

        private async Task RefreshDevicesAsync()
        {
            this.IsBusy = true;
            this.Devices = await this._deployService.GetDevicesAsync();

            this.IsBusy = false;
        }
    }
}