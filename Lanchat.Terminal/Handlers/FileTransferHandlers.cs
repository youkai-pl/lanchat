using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.Handlers
{
    public class FileTransferHandlers
    {
        private readonly INode node;
        private FileTransferStatus fileTransferStatus;

        public FileTransferHandlers(INode node)
        {
            this.node = node;
            node.FileSender.FileTransferQueued += OnFileTransferQueued;
            node.FileSender.FileTransferRequestRejected += FileSenderOnFileTransferRequestRejected;
            node.FileSender.FileTransferError += OnFileTransferError;
            node.FileReceiver.FileTransferRequestReceived += OnFileTransferQueued;
            node.FileReceiver.FileTransferError += OnFileTransferError;
        }

        private void OnFileTransferQueued(object sender, CurrentFileTransfer e)
        {
            fileTransferStatus = new FileTransferStatus(node, e, TabsManager.FileTransfersView.Counter);
            TabsManager.FileTransfersView.Add(fileTransferStatus);
            TabsManager.SignalFileTransfer();
            e.PropertyChanged += (_, _) => fileTransferStatus.Update(e.Progress == 100 ? Resources.FileTransferFinished : $"{e.Progress}%");
        }

        private void FileSenderOnFileTransferRequestRejected(object sender, CurrentFileTransfer e)
        {
            fileTransferStatus.Update(Resources.FileTransferRejected);
            TabsManager.SignalFileTransfer();
        }

        private void OnFileTransferError(object sender, FileTransferException e)
        {
            fileTransferStatus.Update(e.Message);
            TabsManager.SignalFileTransfer();
        }
    }
}