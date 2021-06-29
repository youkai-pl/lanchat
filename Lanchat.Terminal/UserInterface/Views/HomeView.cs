using System.Reflection;
using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class HomeView : SimpleControl
    {
        private readonly VerticalStackPanel stackPanel;

        public HomeView()
        {
            stackPanel = new VerticalStackPanel
            {
                Children = new []
                {
                    new BreakPanel
                    {
                        Content = new TextBlock
                        {
                            Text = Resources._Logo
                        }
                    }
                }
            };
            
            Content = new Box
            {
                HorizontalContentPlacement = Box.HorizontalPlacement.Center,
                VerticalContentPlacement = Box.VerticalPlacement.Center,
                Content = new Boundary
                {
                    Width = 43,
                    Content = stackPanel
                }
            };

            var currentVersion = Assembly.GetEntryAssembly().GetName().Version.ToString(3);
            stackPanel.Add(new TextBlock{Text = $"Version: {currentVersion}"});
        }

        public void AddAlert(string message)
        {
            stackPanel.Add(new TextBlock{Text = message});
        }
    }
}