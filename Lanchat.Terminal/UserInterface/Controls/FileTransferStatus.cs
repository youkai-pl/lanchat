using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class FileTransferStatus : SimpleControl
    {
        private readonly TextBlock textBlock = new();
        private readonly INode node;
        private readonly CurrentFileTransfer currentFileTransfer;

        public FileTransferStatus(INode node, CurrentFileTransfer currentFileTransfer)
        {
            this.node = node;
            this.currentFileTransfer = currentFileTransfer;
            Update();
        }

        public void Update()
        {
            textBlock.Text =
                $"{node.User.Nickname} - {currentFileTransfer.FileName} - {currentFileTransfer.PartsTransferred}/{currentFileTransfer.Parts}";
            Content = textBlock;
        }
    }
}