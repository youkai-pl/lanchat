using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class DebugView : SimpleControl, IScrollable
    {
        private readonly VerticalStackPanel stackPanel;
        public VerticalScrollPanel ScrollPanel { get; set; }

        public DebugView()
        {
            stackPanel = new VerticalStackPanel();
            Content = stackPanel;
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