using System.Threading;
using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class HomeView : SimpleControl
    {
        private readonly TextBlock text;
        private readonly VerticalStackPanel stackPanel;

        public HomeView()
        {
            text = new TextBlock();

            stackPanel = new VerticalStackPanel
            {
                Children = new []
                {
                    new BreakPanel
                    {
                        Content = text
                    }
                }
            };
            Content = new Box
            {
                HorizontalContentPlacement = Box.HorizontalPlacement.Center,
                VerticalContentPlacement = Box.VerticalPlacement.Center,
                Content = new Boundary
                {
                    Width = 59,
                    Content = stackPanel
                }
            };
        }

        public void Animate()
        {
            Thread.Sleep(100);
            text.Text = Resources._Logo1;
            Thread.Sleep(100);
            text.Text = Resources._Logo2;
            Thread.Sleep(100);
            text.Text = Resources._Logo3;
            Thread.Sleep(100);
            text.Text = Resources._Logo4;
            Thread.Sleep(100);
            stackPanel.Add(new TextBlock{Text = "Looking for nodes..."});
        }
    }
}