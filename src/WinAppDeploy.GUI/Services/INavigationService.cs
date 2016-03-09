using System.Windows.Controls;

namespace WinAppDeploy.GUI.Services
{
    public enum PageKey
    {
        SDKError,
        DevicesPage,
        SplashView
    }

    public interface INavigationService
    {
        bool CanGoBack { get; }

        void InitializeFrame(Frame mainFrame);

        void GoBack();

        void NavigateTo(PageKey page);

        void NavigateTo(PageKey page, object parameters);
    }
}