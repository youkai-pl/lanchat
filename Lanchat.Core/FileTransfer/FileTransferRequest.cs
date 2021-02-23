using System.ComponentModel;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    public class FileTransferRequest : INotifyPropertyChanged
    {
        private long partsTransferred;

        internal bool Accepted { get; set; }
        public string FilePath { get; internal init; }
        public string FileName => Path.GetFileName(FilePath);
        public long Parts { get; internal init; }

        public long PartsTransferred
        {
            get => partsTransferred;
            internal set
            {
                if (partsTransferred == value) return;
                partsTransferred = value;
                OnPropertyChanged();
            }
        }

        public long Progress => 100 * PartsTransferred / Parts;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}