using System.Collections.Generic;
using System.Reflection;
using ConsoleGUI;
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
            var currentVersion = $"Version: {Assembly.GetEntryAssembly()!.GetName().Version!.ToString(3)}";

            stackPanel = new VerticalStackPanel
            {
                Children = new List<IControl>
                {
                    new BreakPanel
                    {
                        Content = new TextBlock
                        {
                            Text = string.Format(Resources._Logo, currentVersion)
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
                    Width = 65,
                    Content = stackPanel
                }
            };
        }

        public void AddText(string text, Color color)
        {
            Window.UiAction(() => stackPanel.Add(new TextBlock {Text = text, Color = color}));
        }
    }
}