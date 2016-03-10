using System;
using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Squirrel;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDeployService _deployService;

        public SplashViewModel(INavigationService navigationService, IDeployService deployService)
        {
            this._navigationService = navigationService;
            this._deployService = deployService;

            this.CreateCommands();

        }

       

        public ICommand RefreshCommand { get; private set; }
        public ICommand InstallSdkCommand { get; private set; }

        private void CreateCommands()
        {
            this.RefreshCommand = new RelayCommand(async () => { await this.InitializeAsync(); });
            this.InstallSdkCommand = new RelayCommand(this.OpenSdkWebSite);
        }

        private void OpenSdkWebSite()
        {
            Process.Start("https://dev.windows.com/en-us/downloads/windows-10-sdk");
        }

        public async Task InitializeAsync()
        {
            // Check for app updates
            await this.CheckForUpdatesAsync();

            // Check for the Windows 10 SDK
            var isInstalled = await this._deployService.IsSDKInstalledAsync();
            this._navigationService.NavigateTo(isInstalled ? PageKey.DevicesPage : PageKey.SDKError);
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                var updateUrl = "https://github.com/cjgaliana/Win10AppDeploy";
                using (var updateManager = UpdateManager.GitHubUpdateManager(updateUrl))
                {
                    await updateManager.Result.UpdateApp();
                }
            }
            catch (Exception ex)
            {
                // Silent errors
            }
          
        }
    }
}