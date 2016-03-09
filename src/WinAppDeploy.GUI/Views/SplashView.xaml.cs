using System.Windows.Controls;
using WinAppDeploy.GUI.ViewModels;

namespace WinAppDeploy.GUI.Views
{
    /// <summary>
    ///     Interaction logic for SplashView.xaml
    /// </summary>
    public partial class SplashView : UserControl
    {
        public SplashView()
        {
            this.InitializeComponent();
            this.Loaded += (s, e) => { this.ViewModel.InitializeAsync(); };
        }

        private SplashViewModel ViewModel => this.DataContext as SplashViewModel;
    }
}