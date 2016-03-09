using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using WinAppDeploy.GUI.Models;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModels
{
    public class DeviceDetailsViewModel : BaseViewModel
    {
        private readonly IDeployService _deployService;
        private readonly INavigationService _navigationService;

        public DeviceDetailsViewModel(IDeployService deployService, INavigationService navigationService)
        {
            this._deployService = deployService;
            this._navigationService = navigationService;

            this.CreateCommands();
        }

        private void CreateCommands()
        {
            this.InstallNewAppCommand = new RelayCommand(async()=> { await this.InstallAppAsync(); });
        }

        private async Task InstallAppAsync()
        {
            var appPath = this.PickFile();
            if (string.IsNullOrWhiteSpace(appPath))
            {
                return;
            }
            await this._deployService.InstallAppAsync(appPath, this.Device);
        }

        private string PickFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "App Packages (*.appx)|*.appx|All files (*.*)|*.*"; 
            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileName;
            }

            return "";
        }


        public ICommand InstallNewAppCommand { get; private set; }

        public DeployTargetDevice Device
        {
            get { return this._device; }
            set { this.Set(() => this.Device, ref this._device, value); }
        }

        public IList<WinApp> InstalledApps
        {
            get { return this._installedApps; }
            set { this.Set(() => this.InstalledApps, ref this._installedApps, value); }
        }

        public WinApp SelectedApp
        {
            get { return this._selectedApp; }
            set { this.Set(() => this.SelectedApp, ref this._selectedApp, value); }
        }

        public async Task InitializeAsync()
        {
            var device = this._navigationService.NavigationParameter as DeployTargetDevice;
            if (device == null)
            {
                throw new ArgumentNullException();
            }

            this.Device = device;
            await this.LoadAppsAsync();
        }

        private async Task LoadAppsAsync()
        {
            this.IsBusy = true;
            var apps = await this._deployService.GetInstalledAppsAsync(this.Device);
            this.InstalledApps = apps;
            this.IsBusy = false;
        }

        private DeployTargetDevice _device;
        private IList<WinApp> _installedApps;
        private WinApp _selectedApp;
    }
}