using System.ComponentModel;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Terminal.UserInterface
{
    public class FileTransferProgressMonitor : INotifyPropertyChanged
    {
        private int fileTransfersInProgress;

        public string Text => $"File transfers: {fileTransfersInProgress}";

        internal void ObserveNodeTransfers(FileReceiver fileReceiver, FileSender fileSender)
        {
            fileReceiver.PropertyChanged += (_, _) =>
            {
                if (fileReceiver.Request != null)
                {
                    fileTransfersInProgress++;
                }
                else
                {
                    fileTransfersInProgress--;
                }
                OnPropertyChanged(nameof(Text));
            };
            
            fileSender.PropertyChanged += (_, _) =>
            {
                if (fileSender.Request != null)
                {
                    fileTransfersInProgress++;
                }
                else
                {
                    fileTransfersInProgress--;
                }
                
                OnPropertyChanged(nameof(Text));
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}