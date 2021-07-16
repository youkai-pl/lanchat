using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Handlers
{
    public class FileTransferHandlers
    {
        private readonly INode node;
        private readonly FileTransfersView fileTransfersView;

        public FileTransferHandlers(INode node, FileTransfersView fileTransfersView)
        {
            this.node = node;
            this.fileTransfersView = fileTransfersView;
            node.FileSender.FileTransferQueued += FileSenderOnFileTransferQueued;
            node.FileReceiver.FileTransferRequestReceived += FileSenderOnFileTransferQueued;
        }

        private void FileSenderOnFileTransferQueued(object sender, CurrentFileTransfer e)
        {
            var fileTransferStatus = new FileTransferStatus(node, e);
            fileTransfersView.Add(fileTransferStatus);
            e.PropertyChanged += (_, _) =>
            {
                fileTransferStatus.Update();
            };
        }
    }
}