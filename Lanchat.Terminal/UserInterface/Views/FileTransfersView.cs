using System;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
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
                Content = stackPanel,
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
            Window.UiAction(() => stackPanel.Add(fileTransferStatus));
            Counter++;
        }
    }
}