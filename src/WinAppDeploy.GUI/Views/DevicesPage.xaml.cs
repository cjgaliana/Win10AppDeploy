using System.Windows.Controls;
using WinAppDeploy.GUI.ViewModels;

namespace WinAppDeploy.GUI.Views
{
    /// <summary>
    /// Interaction logic for DevicesPage.xaml
    /// </summary>
    public partial class DevicesPage : UserControl
    {
        private MainViewModel ViewModel => this.DataContext as MainViewModel;

        public DevicesPage()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                this.ViewModel?.InitializeAsync();
            };
        }
    }
}