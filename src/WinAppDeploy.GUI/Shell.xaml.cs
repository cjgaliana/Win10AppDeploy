using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using WinAppDeploy.GUI.Messages;

namespace WinAppDeploy.GUI
{
    /// <summary>
    ///     Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell
    {
        public Shell()
        {
            this.InitializeComponent();

            Messenger.Default.Register<BusyMessage>(this, this.UpdateBusyIndicator);
        }

        private void UpdateBusyIndicator(BusyMessage message)
        {
            this.BusyIndicator.Visibility = message.IsBusy ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}