using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class FileTransferStatus : SimpleControl
    {
        private readonly int counter;
        private readonly CurrentFileTransfer currentFileTransfer;
        private readonly INode node;
        private readonly TextBlock textBlock = new();

        public FileTransferStatus(INode node, CurrentFileTransfer currentFileTransfer, int counter)
        {
            this.node = node;
            this.currentFileTransfer = currentFileTransfer;
            this.counter = counter;
            Update();
        }

        public void Update()
        {
            Window.UiAction(() =>
            {
                textBlock.Text =
                    $"#{counter} | {node.User.Nickname} - {currentFileTransfer.FileName} - {currentFileTransfer.Progress}%";
                Content = textBlock;
            });
        }
    }
}