using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using WinAppDeploy.GUI.Messages;

namespace WinAppDeploy.GUI.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private bool _isBusy;

        public bool IsBusy
        {
            get { return this._isBusy; }
            set
            {
                this.Set(() => this.IsBusy, ref this._isBusy, value);

                Messenger.Default.Send(new BusyMessage
                {
                    IsBusy = value
                });
            }
        }
    }
}