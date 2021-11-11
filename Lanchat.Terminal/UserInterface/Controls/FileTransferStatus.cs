using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class FileTransferStatus : SimpleControl
    {
        private readonly int counter;
        private readonly CurrentFileTransfer currentFileTransfer;
        private readonly TextBlock textBlock = new();

        public FileTransferStatus(INode node, CurrentFileTransfer currentFileTransfer, int counter)
        {
            Node = node;
            this.currentFileTransfer = currentFileTransfer;
            this.counter = counter;
            Update(Resources._FileTransferWaiting);
        }

        public INode Node { get; }

        public void Update(string status)
        {
            Window.UiAction(() =>
            {
                textBlock.Text =
                    $"#{counter} | {Node.User.Nickname}#{Node.User.ShortId}- {currentFileTransfer.FileName} - {status}";
                Content = textBlock;
            });
        }
    }
}