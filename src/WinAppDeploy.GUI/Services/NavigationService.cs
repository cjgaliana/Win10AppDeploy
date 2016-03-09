using System;
using System.Collections.Generic;
using System.Windows.Controls;
using WinAppDeploy.GUI.Views;

namespace WinAppDeploy.GUI.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _currentFrame;
        private readonly Dictionary<PageKey, Type> _pages;

        public NavigationService()
        {
            // Map pages
            this._pages = new Dictionary<PageKey, Type>
            {
                {PageKey.DevicesPage, typeof (DevicesPage)},
                {PageKey.SDKError, typeof (SDKErrorView)},
                {PageKey.SplashView, typeof (SplashView)}
            };
        }

        public void InitializeFrame(Frame mainFrame)
        {
            if (mainFrame == null)
            {
                throw new ArgumentNullException(nameof(mainFrame));
            }

            this._currentFrame = mainFrame;
        }

        public bool CanGoBack => this._currentFrame.CanGoBack;

        public void GoBack()
        {
            if (this.CanGoBack)
            {
                this._currentFrame.GoBack();
            }
        }

        public void NavigateTo(PageKey page)
        {
            this.NavigateTo(page, null);
        }

        public void NavigateTo(PageKey page, object parameters)
        {
            if (!this._pages.ContainsKey(page))
            {
                return;
            }

            var pageType = this._pages[page];

            this.NavigateToPage(pageType, parameters);
        }

        private void NavigateToPage(Type page, object parameter = null)
        {
            if (this._currentFrame?.Content != null)
            {
                var currentPageType = this._currentFrame.Content.GetType();

                if (page.Name == currentPageType.Name)
                {
                    //Cancel navigation
                    return;
                }
            }

            this._currentFrame.Navigate(new Uri($"/Views/{page.Name}.xaml", UriKind.Relative), parameter);
        }
    }
}