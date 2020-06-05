using Lanchat.Common.NetworkLib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lanchat.Xamarin.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private string input = string.Empty;

        public ChatViewModel(Network network)
        {
            Send = new Command(SendAction);
            Network = network;

            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += (sender, e) =>
            {
            };

            var NetworkEventsHandlers = new NetworkEventsHandlers(this);
            Network.Events.ReceivedMessage += NetworkEventsHandlers.OnReceivedMessage;
            Network.Events.NodeConnected += NetworkEventsHandlers.OnNodeConnected;
            Network.Events.NodeDisconnected += NetworkEventsHandlers.OnNodeDisconnected;
            Network.Events.ChangedNickname += NetworkEventsHandlers.OnChangedNickname;
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

        public ObservableCollection<Message> Messages { get; set; }

        public Network Network { get; private set; }
        public ICommand Send { get; private set; }

        public void AddMessage(Message message)
        {
            Messages.Add(message);
            MessagingCenter.Send<object>(this, "LogUpdated");
        }

        private void OnPropertyChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SendAction()
        {
            Network.Methods.SendAll(Input);
            AddMessage(new Message() { Content = Input, Nickname = Network.Nickname });
            Input = string.Empty;
        }
    }

    public class Message
    {
        public string Content { get; set; }
        public string Nickname { get; set; }
    }
}