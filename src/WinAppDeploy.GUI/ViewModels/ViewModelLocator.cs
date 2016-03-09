using Microsoft.Practices.ServiceLocation;

namespace WinAppDeploy.GUI.ViewModels
{
    public class ViewModelLocator
    {
        public SplashViewModel SplashViewModel => ServiceLocator.Current.GetInstance<SplashViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public DeviceDetailsViewModel DeviceDetailsViewModel => ServiceLocator.Current.GetInstance<DeviceDetailsViewModel>();
    }
}