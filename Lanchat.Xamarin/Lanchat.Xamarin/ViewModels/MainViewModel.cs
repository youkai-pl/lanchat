using Lanchat.Common.NetworkLib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lanchat.Xamarin.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string input = string.Empty;

        public MainViewModel()
        {
            Send = new Command(SendAction);

            Task.Run(() =>
             {
                 Network = new Network(4001, "Xamarin", 4002, 3000);
                 var NetworkEventsHandlers = new NetworkEventsHandlers(this);
                 Network.Events.HostStarted += NetworkEventsHandlers.OnHostStarted;
                 Network.Events.ReceivedMessage += NetworkEventsHandlers.OnReceivedMessage;
                 Network.Events.NodeConnected += NetworkEventsHandlers.OnNodeConnected;
                 Network.Events.NodeDisconnected += NetworkEventsHandlers.OnNodeDisconnected;
                 Network.Events.NodeSuspended += NetworkEventsHandlers.OnNodeSuspended;
                 Network.Events.NodeResumed += NetworkEventsHandlers.OnNodeResumed;
                 Network.Events.ChangedNickname += NetworkEventsHandlers.OnChangedNickname;
                 Network.Start();
             });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Input
        {
            get => input;
            set
            {
                if (input == value)
                {
                    return;
                }
                input = value;
                OnPropertyChange(nameof(Input));
            }
        }

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        public Network Network { get; private set; }
        public ICommand Send { get; private set; }

        public void AddMessage(string content)
        {
            Messages.Add(new Message() { Content = content });
        }

        private void OnPropertyChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SendAction()
        {
            Network.Methods.SendAll(Input);
            Messages.Add(new Message() { Content = Input });
            Input = string.Empty;
        }
    }

    public class Message
    {
        public string Content { get; set; }
    }
}