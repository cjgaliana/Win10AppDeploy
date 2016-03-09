using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinAppDeploy.GUI.ViewModels;

namespace WinAppDeploy.GUI.Views
{
    /// <summary>
    /// Interaction logic for DeviceDetailsView.xaml
    /// </summary>
    public partial class DeviceDetailsView : UserControl
    {
        public DeviceDetailsView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { this.ViewModel.InitializeAsync(); };
        }

        private DeviceDetailsViewModel ViewModel => this.DataContext as DeviceDetailsViewModel;
    }
}
