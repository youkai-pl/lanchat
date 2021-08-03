using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.Handlers
{
    public class FileTransferHandlers
    {
        private readonly INode node;

        public FileTransferHandlers(INode node)
        {
            this.node = node;
            node.FileSender.FileTransferQueued += FileSenderOnFileTransferQueued;
            node.FileReceiver.FileTransferRequestReceived += FileSenderOnFileTransferQueued;
            node.FileReceiver.FileTransferRequestReceived += FileReceiverOnFileTransferRequestReceived;
        }

        private void FileSenderOnFileTransferQueued(object sender, CurrentFileTransfer e)
        {
            var fileTransferStatus = new FileTransferStatus(node, e, TabsManager.FileTransfersView.Counter);
            TabsManager.FileTransfersView.Add(fileTransferStatus);
            e.PropertyChanged += (_, _) => { fileTransferStatus.Update(); };
        }

        private static void FileReceiverOnFileTransferRequestReceived(object sender, CurrentFileTransfer e)
        {
            TabsManager.SignalFileTransfer();
        }
    }
}