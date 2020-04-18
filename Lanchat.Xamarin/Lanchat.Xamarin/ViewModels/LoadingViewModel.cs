using Lanchat.Common.NetworkLib;
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
                Network = new Network(4001, "Xamarin", 4002, 3000);
                Network.Events.HostStarted += (sender, e) =>
                {
                    loadingIndicator = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new ChatPage(Network);
                    });
                };
                Network.Start();
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

        public Network Network { get; private set; }

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