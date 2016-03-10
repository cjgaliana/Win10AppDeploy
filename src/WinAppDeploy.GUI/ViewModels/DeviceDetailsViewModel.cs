using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
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
            this.InstallNewAppCommand = new RelayCommand(async () => { await this.InstallAppAsync(); });
            this.UnistallCommand = new RelayCommand(async () => { await this.UnistallAppAsync(); });
            this.UpdateAppCommand = new RelayCommand(async () => { await this.UpdateAppAsync(); });
        }

        private async Task UnistallAppAsync()
        {
            try
            {
                if (this.SelectedApp == null)
                {
                    return;
                }

                this.IsBusy = true;
                await this._deployService.UnistallAppAsync(this.SelectedApp, this.Device);
                await this.LoadAppsAsync();
            }
            catch (Exception ex)
            {
                var a = 5;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task InstallAppAsync()
        {
            var appPath = this.PickFile();
            if (string.IsNullOrWhiteSpace(appPath))
            {
                return;
            }
            try
            {
                this.IsBusy = true;
                await this._deployService.InstallAppAsync(appPath, this.Device);
            }
            catch (Exception ex)
            {
                var a = 5;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private async Task UpdateAppAsync()
        {
            try
            {
                if (this.SelectedApp == null)
                {
                    return;
                }

                var appPath = this.PickFile();
                if (string.IsNullOrWhiteSpace(appPath))
                {
                    return;
                }

                this.IsBusy = true;
                await this._deployService.UpdateAppAsync(appPath, this.Device);
                await this.LoadAppsAsync();
            }
            catch (Exception ex)
            {
                var a = 5;
            }
            finally
            {
                this.IsBusy = false;
            }
        }

        private string PickFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "App Packages (*.appx;*.appxbundle)|*.appx;*appxbundle|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                fileName = Regex.Replace(fileName, "\\\\", "\\");
                return fileName;
            }

            return "";
        }

        public ICommand InstallNewAppCommand { get; private set; }
        public ICommand UnistallCommand { get; private set; }
        public ICommand UpdateAppCommand { get; private set; }

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

        public IList<WinApp> FilteredInstalledApps
        {
            get { return this._filteredInstalledApps; }
            set { this.Set(() => this.FilteredInstalledApps, ref this._filteredInstalledApps, value); }
        }

        public string QueryString
        {
            get { return this._queryString; }
            set
            {
                this.Set(() => this.QueryString, ref this._queryString, value);
                this.FilterAppsAsync(value);
            }
        }

        private Task FilterAppsAsync(string query)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    this.FilteredInstalledApps = this.InstalledApps;
                    return;
                }

                var filtered = this.InstalledApps
                    .Where(x => x.PackageName.ToLowerInvariant().Contains(query.ToLowerInvariant()))
                    .ToList();
                this.FilteredInstalledApps = filtered;
            });
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
            this.QueryString = string.Empty;
            this.IsBusy = false;
        }

        private DeployTargetDevice _device;
        private IList<WinApp> _installedApps;
        private IList<WinApp> _filteredInstalledApps;
        private string _queryString;
        private WinApp _selectedApp;
    }
}