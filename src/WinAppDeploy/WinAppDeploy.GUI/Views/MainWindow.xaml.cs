using System.Threading.Tasks;
using MahApps.Metro.Controls;
using WinAppDeploy.GUI.ViewModel;

namespace WinAppDeploy.GUI
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel ViewModel => this.DataContext as MainViewModel;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Loaded += async (e, s) => { await this.ViewModel.InitializeAsync(); };
        }

        private async void ReInitialiseViewModel(object sender, System.Windows.RoutedEventArgs e)
        {
            await this.ViewModel.InitializeAsync();
        }

        private void TestClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.ViewModel.TestCommand.Execute(null);
        }
    }
}