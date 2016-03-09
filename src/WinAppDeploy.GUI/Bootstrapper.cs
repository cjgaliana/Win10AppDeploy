using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using WinAppDeploy.GUI.Services;
using WinAppDeploy.GUI.ViewModels;

namespace WinAppDeploy.GUI
{
    public class Bootstrapper
    {
        private readonly UnityContainer _container;

        public Bootstrapper()
        {
            this._container = new UnityContainer();
        }

        public void Run()
        {
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(this._container));

            this.RegisterServices();
            this.RegisterViewModels();

            var shell = new Shell();
            App.Current.MainWindow = shell;
            App.Current.MainWindow.Show();

            // Initialise frame
            var navService = ServiceLocator.Current.GetInstance<INavigationService>();
            if (navService != null)
            {
                navService.InitializeFrame(shell.MainFrame);
                navService.NavigateTo(PageKey.SplashView);
            }
        }

        private void RegisterServices()
        {
            this._container.RegisterType<IDeployService, Win10DeployService>(new ContainerControlledLifetimeManager());
            this._container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
        }

        private void RegisterViewModels()
        {
            this._container.RegisterType<SplashViewModel>();
            this._container.RegisterType<MainViewModel>();
            this._container.RegisterType<DeviceDetailsViewModel>();
        }
    }
}