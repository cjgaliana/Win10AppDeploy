using GalaSoft.MvvmLight.CommandWpf;
using Squirrel;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModels
{
    public class SplashViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDeployService _deployService;
        private int _updateProgress;
        private string _loadingMessage;

        public SplashViewModel(INavigationService navigationService, IDeployService deployService)
        {
            this._navigationService = navigationService;
            this._deployService = deployService;

            this.CreateCommands();
        }

        public ICommand RefreshCommand { get; private set; }
        public ICommand InstallSdkCommand { get; private set; }

        public int UpdateProgress
        {
            get { return this._updateProgress; }
            set { this.Set(() => this.UpdateProgress, ref this._updateProgress, value); }
        }

        public string LoadingMessage
        {
            get { return this._loadingMessage; }
            set { this.Set(() => this.LoadingMessage, ref this._loadingMessage, value); }
        }

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
            this.LoadingMessage = "Checking Windows 10 SDK";
            var isInstalled = await this._deployService.IsSDKInstalledAsync();
            this._navigationService.NavigateTo(isInstalled ? PageKey.DevicesPage : PageKey.SDKError);
        }

        private async Task CheckForUpdatesAsync()
        {
            try
            {
                this.LoadingMessage = $"Checking for app updates";
                var updateUrl = "https://github.com/cjgaliana/Win10AppDeploy";
                using (var updateManager = await UpdateManager.GitHubUpdateManager(updateUrl))
                {
                    await updateManager.UpdateApp((progress) =>
                    {
                        this.UpdateProgress = progress;
                        this.LoadingMessage = $"Downloading Update: {this.UpdateProgress}%";
                    });
                }
            }
            catch (Exception ex)
            {
                // Silent errors?
                var a = 5;
            }
        }
    }
}