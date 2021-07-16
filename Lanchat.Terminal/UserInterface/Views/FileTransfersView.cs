using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class FileTransfersView : SimpleControl
    {
        private readonly VerticalStackPanel stackPanel = new();
        public int Counter { get; private set; } = 1;
        public FileTransfersView()
        {
            Content = stackPanel;
        }

        public void Add(FileTransferStatus fileTransferStatus)
        {
            stackPanel.Add(fileTransferStatus);
            Counter++;
        }
    }
}