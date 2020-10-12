using Lanchat.Core;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lanchat.Xamarin.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private string input = string.Empty;

        public ChatViewModel(P2P network)
        {
            Network = network;
            Send = new Command(SendAction);
            Messages = new ObservableCollection<Message>();
            Messages.CollectionChanged += (sender, e) =>
            {
            };

            Network.ConnectionCreated += (sender, e) =>
            {
                _ = new NetworkEventsHandlers(this, e);
            };
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

        public P2P Network { get; private set; }
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
            Network.BroadcastMessage(Input);
            AddMessage(new Message() { Content = Input, Nickname = Config.Nickname });
            Input = string.Empty;
        }
    }

    public class Message
    {
        public string Content { get; set; }
        public string Nickname { get; set; }
    }
}