using System;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class FileTransfersView : SimpleControl, IScrollable
    {
        private readonly VerticalStackPanel stackPanel = new();

        public FileTransfersView()
        {
            ScrollPanel = new VerticalScrollPanel
            {
                Content = new TextBlock { Text = Resources._NoFileReceiveRequest },
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character(),
                ScrollUpKey = ConsoleKey.PageUp,
                ScrollDownKey = ConsoleKey.PageDown
            };

            Content = ScrollPanel;
        }

        public int Counter { get; private set; } = 1;

        public VerticalScrollPanel ScrollPanel { get; }

        public void Add(FileTransferStatus fileTransferStatus)
        {
            ScrollPanel.Content = stackPanel;
            Window.UiAction(() => stackPanel.Add(fileTransferStatus));
            Counter++;
        }

        public FileTransferStatus GetFileTransferStatus(INode node)
        {
            return stackPanel.Children.Cast<FileTransferStatus>().FirstOrDefault(x => x.Node == node);
        }
    }
}