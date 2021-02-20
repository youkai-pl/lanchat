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
            fileSender.FileTransferRequestAccepted += (_, _) =>
            {
                fileTransfersInProgress++;
                OnPropertyChanged();
            };

            fileSender.FileTransferFinished += (_, _) =>
            {
                fileTransfersInProgress--;
                OnPropertyChanged();
            };

            fileSender.FileTransferError += (_, _) =>
            {
                fileTransfersInProgress--;
                OnPropertyChanged();
            };
            
            fileReceiver.FileTransferStarted += (_, _) =>
            {
                fileTransfersInProgress++;
                OnPropertyChanged();
            };
            
            fileReceiver.FileTransferFinished += (_, _) =>
            {
                fileTransfersInProgress--;
                OnPropertyChanged();
            };
            
            fileReceiver.FileTransferError += (_, _) =>
            {
                fileTransfersInProgress--;
                OnPropertyChanged();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}