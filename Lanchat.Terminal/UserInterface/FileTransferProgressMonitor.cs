using System.ComponentModel;
using Lanchat.Core.FileTransfer;

namespace Lanchat.Terminal.UserInterface
{
    public class FileTransferProgressMonitor : INotifyPropertyChanged
    {
        private int fileTransfersInProgress;
        private long progress;

        public string Text => $"File transfers: {progress}% / {fileTransfersInProgress} ";

        internal void ObserveNodeTransfers(FileReceiver fileReceiver, FileSender fileSender)
        {
            fileReceiver.PropertyChanged += (_, _) =>
            {
                if (fileReceiver.Request != null)
                {
                    fileTransfersInProgress++;
                    fileReceiver.Request.PropertyChanged += (_, _) =>
                    {
                        progress = fileReceiver.Request.Progress;
                        OnPropertyChanged();
                    };
                }
                else
                {
                    progress = 0;
                    fileTransfersInProgress--;
                }

                OnPropertyChanged();
            };

            fileSender.PropertyChanged += (_, _) =>
            {
                if (fileSender.Request != null)
                {
                    fileTransfersInProgress++;
                    fileSender.Request.PropertyChanged += (_, _) =>
                    {
                        progress = fileSender.Request.Progress;
                        OnPropertyChanged();
                    };
                }
                else
                {
                    progress = 0;
                    fileTransfersInProgress--;
                }

                OnPropertyChanged();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}