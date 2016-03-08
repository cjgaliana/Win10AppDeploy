using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using WinAppDeploy.GUI.Views;

namespace WinAppDeploy.GUI.Modules
{
    public class TestModule : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public TestModule(IUnityContainer container, IRegionManager regionManager)
        {
            this._container = container;
            this._regionManager = regionManager;
        }

        public void Initialize()
        {
            //this._regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, typeof(PhoneDetailsView));
            this._regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(MainView));
        }
    }
}