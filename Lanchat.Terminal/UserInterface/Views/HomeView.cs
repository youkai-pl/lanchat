using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class HomeView : SimpleControl
    {
        public HomeView()
        {
            Content = new Box
            {
                HorizontalContentPlacement = Box.HorizontalPlacement.Center,
                VerticalContentPlacement = Box.VerticalPlacement.Center,
                Content = new Boundary
                {
                    Width = 43,
                    Height = 6,
                    Content = new BreakPanel
                    {
                        Content = new TextBlock
                        {
                            Text = Resources._Logo
                        }
                    }
                }
            };
        }
    }
}