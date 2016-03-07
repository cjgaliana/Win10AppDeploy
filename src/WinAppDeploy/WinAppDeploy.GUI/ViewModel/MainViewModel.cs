using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDeployService _deployService;

        private bool _isCommandInstalled;
        private bool _isBusy;

        public MainViewModel(IDeployService deployService)
        {
            this._deployService = deployService;

            this.IsCommandInstalled = false;

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

        private void CreateCommands()
        {
            this.RefreshDevicesCommand = new RelayCommand(async () => { await this.RefreshDevicesAsync(); });
            this.InstallSdkCommand = new RelayCommand(this.OpenSdkWebSite);
            //this.SettingsCommand = new RelayCommand(Open Settings page); 
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
            var devices = await this._deployService.GetDevicesAsync();
            this.IsBusy = false;
        }
    }
}