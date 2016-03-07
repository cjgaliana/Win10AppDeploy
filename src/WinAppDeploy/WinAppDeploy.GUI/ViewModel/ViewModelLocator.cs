using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using WinAppDeploy.GUI.Services;

namespace WinAppDeploy.GUI.ViewModel
{
    /// <summary>
    ///     This class contains static references to all the view models in the
    ///     application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly UnityContainer _container;

        /// <summary>
        ///     Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            this._container = new UnityContainer();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(this._container));

            this.RegisterServices();
            this.RegisterViewModels();
        }

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        private void RegisterServices()
        {
            this._container.RegisterType<IDeployService, Win10DeployService>();
        }

        private void RegisterViewModels()
        {
            this._container.RegisterType<MainViewModel>();
        }
    }
}