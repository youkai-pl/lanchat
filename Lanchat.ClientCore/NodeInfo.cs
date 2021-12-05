using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core.Config;

namespace Lanchat.ClientCore
{
    /// <inheritdoc cref="INodeInfo" />
    public class NodeInfo : INodeInfo, INotifyPropertyChanged
    {
        private string nickname;
        private IPAddress ipAddress;
        private int id;
        private bool blocked;

        /// <inheritdoc />
        public IPAddress IpAddress
        {
            get => ipAddress;
            set
            {
                ipAddress = value;
                OnPropertyChanged(nameof(IpAddress));
            }
        }

        /// <inheritdoc />
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        /// <inheritdoc />
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <inheritdoc />
        public bool Blocked
        {
            get => blocked;
            set
            {
                blocked = value;
                OnPropertyChanged(nameof(Blocked));
            }
        }

        /// <inheritdoc />
        [JsonIgnore]
        public string PublicKey
        {
            get => NodesDatabase.ReadPemFile(IpAddress.ToString());
            set => NodesDatabase.SavePemFile(IpAddress.ToString(), value);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}