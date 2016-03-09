using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModels
{
    public class ViewModelLocator
    {

     
        public SplashViewModel SplashViewModel => ServiceLocator.Current.GetInstance<SplashViewModel>();
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
       
    
    }
}