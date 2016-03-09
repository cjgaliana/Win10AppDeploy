using GalaSoft.MvvmLight.CommandWpf;
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
            var isInstalled = await this._deployService.IsSDKInstalledAsync();
            this._navigationService.NavigateTo(isInstalled ? PageKey.DevicesPage : PageKey.SDKError);
        }
    }
}