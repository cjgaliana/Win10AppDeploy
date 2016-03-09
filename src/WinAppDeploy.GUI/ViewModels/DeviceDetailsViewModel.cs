using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        }

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