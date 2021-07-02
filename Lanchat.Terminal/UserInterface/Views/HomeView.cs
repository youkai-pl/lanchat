using System.Reflection;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class HomeView : SimpleControl, IWriteable
    {
        private readonly VerticalStackPanel stackPanel;

        public HomeView()
        {
            stackPanel = new VerticalStackPanel
            {
                Children = new[]
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
            AddText($"Version: {currentVersion}", Color.White);
        }

        public void AddText(string text, Color color)
        {
            stackPanel.Add(new TextBlock {Text = text, Color = color});
        }
    }
}