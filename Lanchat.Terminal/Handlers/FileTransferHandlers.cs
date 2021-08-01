using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Handlers
{
    public class FileTransferHandlers
    {
        private readonly FileTransfersView fileTransfersView;
        private readonly INode node;

        public FileTransferHandlers(INode node, FileTransfersView fileTransfersView)
        {
            this.node = node;
            this.fileTransfersView = fileTransfersView;
            node.FileSender.FileTransferQueued += FileSenderOnFileTransferQueued;
            node.FileReceiver.FileTransferRequestReceived += FileSenderOnFileTransferQueued;
            node.FileReceiver.FileTransferRequestReceived += FileReceiverOnFileTransferRequestReceived;
        }
        
        private void FileSenderOnFileTransferQueued(object sender, CurrentFileTransfer e)
        {
            var fileTransferStatus = new FileTransferStatus(node, e, fileTransfersView.Counter);
            fileTransfersView.Add(fileTransferStatus);
            e.PropertyChanged += (_, _) => { fileTransferStatus.Update(); };
        }
        
        private static void FileReceiverOnFileTransferRequestReceived(object sender, CurrentFileTransfer e)
        {
            Window.TabsManager.SignalFileTransfer();
        }
    }
}