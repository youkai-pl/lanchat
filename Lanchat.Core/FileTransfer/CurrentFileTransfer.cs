using System;
using System.ComponentModel;
using System.IO;

namespace Lanchat.Core.FileTransfer
{
    /// <summary>
    ///     Class representing single transfer request.
    /// </summary>
    public class CurrentFileTransfer : INotifyPropertyChanged, IDisposable
    {
        private long partsTransferred;
        internal bool Accepted { get; set; }
        internal FileAccess FileAccess { get; set; }
        internal bool Disposed { get; private set; }

        /// <summary>
        ///     Path when file will be saved or when is sending from.
        /// </summary>
        public string FilePath { get; internal init; }

        /// <summary>
        ///     File name.
        /// </summary>
        public string FileName => Path.GetFileName(FilePath);

        /// <summary>
        ///     Size of file in parts.
        /// </summary>
        public long Parts { get; internal init; }

        /// <summary>
        ///     Already transferred parts counter.
        /// </summary>
        public long PartsTransferred
        {
            get => partsTransferred;
            internal set
            {
                partsTransferred = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Raised for <see cref="PartsTransferred" /> update.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <inheritdoc />
        public void Dispose()
        {
            FileAccess?.Dispose();
            Disposed = true;
            GC.SuppressFinalize(this);
        }
    }
}