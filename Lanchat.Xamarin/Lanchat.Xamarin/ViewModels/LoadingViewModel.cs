using Lanchat.Core;
using Lanchat.Xamarin.Pages;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Lanchat.Xamarin.ViewModels
{
    public class LoadingViewModel : INotifyPropertyChanged
    {
        private bool loadingIndicator = true;

        private string status = "Starting Lanchat";

        public LoadingViewModel()
        {
            Task.Run(() =>
            {
                Status = "Starting network";
                Config.Nickname = "Android";
                Network = new P2P(3645);
                Network.Start();

                loadingIndicator = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new ChatPage(Network);
                });
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool LoadingIndicator
        {
            get => loadingIndicator;
            set
            {
                loadingIndicator = value;
                OnPropertyChange(nameof(LoadingIndicator));
            }
        }

        public P2P Network { get; private set; }

        public string Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChange(nameof(Status));
            }
        }

        private void OnPropertyChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}