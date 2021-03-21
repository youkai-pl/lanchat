using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    public class Broadcast : INotifyPropertyChanged
    {
        private string nickname;

        public string Guid { get; set; }

        public string Nickname
        {
            get => nickname;
            set
            {
                if (value == nickname) return;
                nickname = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public IPAddress IpAddress { get; set; }
        [JsonIgnore] public bool Active { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}