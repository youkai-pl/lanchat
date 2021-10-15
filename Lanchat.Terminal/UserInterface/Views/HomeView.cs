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
            stackPanel = new VerticalStackPanel();
            Content = new Box
            {
                HorizontalContentPlacement = Box.HorizontalPlacement.Center,
                VerticalContentPlacement = Box.VerticalPlacement.Center,
                Content = new Boundary
                {
                    Width = 44,
                    Content = stackPanel
                }
            };

            WriteAsciiLogo();
            WriteWelcomeText();
        }

        public void AddText(string text, Color color)
        {
            Window.UiAction(() => stackPanel.Add(new TextBlock { Text = text, Color = color }));
        }

        private void WriteAsciiLogo()
        {
            var currentVersion = $"Version: {Assembly.GetEntryAssembly()!.GetName().Version!.ToString(3)}";
            stackPanel.Add(new BreakPanel
            {
                Content = new TextBlock
                {
                    Text = string.Format(Resources._Logo, currentVersion)
                }
            });
        }

        private void WriteWelcomeText()
        {
            AddText("", Color.White);
            if (Program.Config.Fresh)
            {
                AddText(Resources._GetHelp, Color.White);
            }
        }
    }
}