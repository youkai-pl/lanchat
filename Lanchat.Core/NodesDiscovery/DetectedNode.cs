using System.ComponentModel;
using System.Net;

namespace Lanchat.Core.NodesDiscovery
{
    /// <summary>
    ///     Node detected in LAN but not connected
    /// </summary>
    public class DetectedNode : INotifyPropertyChanged
    {
        private string nickname;

        /// <summary>
        ///     Nickname. Can raise <see cref="PropertyChanged" /> event.
        /// </summary>
        public string Nickname
        {
            get => nickname;
            set
            {
                if (nickname == value)
                {
                    return;
                }

                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <summary>
        ///     IP address of node
        /// </summary>
        public IPAddress IpAddress { get; init; }

        /// <summary>
        ///     Turns to false if node stops sending broadcast.
        /// </summary>
        public bool Active { get; set; }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}