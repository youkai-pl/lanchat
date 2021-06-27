using System;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class DebugView : SimpleControl, IScrollable
    {
        private readonly VerticalStackPanel stackPanel;
        public VerticalScrollPanel ScrollPanel { get; }

        public DebugView()
        {
            stackPanel = new VerticalStackPanel();
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
        
        public void AddToLog(string textBlocks)
        {
            stackPanel.Add(new WrapPanel
            {
                Content = new TextBlock
                {
                    Text = textBlocks
                }
            });
           ScrollPanel.Top = int.MaxValue;
        }
    }
}