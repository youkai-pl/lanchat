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
        public int Counter { get; private set; } = 1;
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

        public void Add(FileTransferStatus fileTransferStatus)
        {
            Window.UiAction(() => stackPanel.Add(fileTransferStatus)); 
            Counter++;
        }

        public VerticalScrollPanel ScrollPanel { get; }
    }
}