using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDeployService _deployService;

        private bool _isBusy;
        private bool _isCommandInstalled;
        private IList<DeployTargetDevice> _devices;
        private DeployTargetDevice _selectedDevice;

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
            set { this.Set(() => this.IsCommandInstalled, ref this._isCommandInstalled, value); }
        }

        public bool IsBusy
        {
            get { return this._isBusy; }
            set { this.Set(() => this.IsBusy, ref this._isBusy, value); }
        }

        public IList<DeployTargetDevice> Devices
        {
            get { return this._devices; }
            set { this.Set(() => this.Devices, ref this._devices, value); }
        }

        public DeployTargetDevice SelectedDevice
        {
            get { return this._selectedDevice; }
            set { this.Set(() => this.SelectedDevice, ref this._selectedDevice, value); }
        }

        public ICommand TestCommand { get; private set; }

        private void CreateCommands()
        {
            this.RefreshDevicesCommand = new RelayCommand(async () => { await this.RefreshDevicesAsync(); });
            this.InstallSdkCommand = new RelayCommand(this.OpenSdkWebSite);
            //this.SettingsCommand = new RelayCommand(Open Settings page);
            this.TestCommand = new RelayCommand(async()=> { await this.Test(); });
        }

        private async Task Test()
        {
            this.IsBusy = true;
            var apps = await this._deployService.GetInstalledAppsAsync(this.SelectedDevice);
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