using System.Windows.Controls;

namespace WinAppDeploy.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            //this.DataContext = new MainViewModel(new Win10DeployService());
            //this.Loaded += async (e, s) => { await this.ViewModel.InitializeAsync(); };
        }

        //private MainViewModel ViewModel => this.DataContext as MainViewModel;

        private async void ReInitialiseViewModel(object sender, System.Windows.RoutedEventArgs e)
        {
            //await this.ViewModel.InitializeAsync();
        }

        private void TestClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //this.ViewModel.TestCommand.Execute(null);
        }
    }
}