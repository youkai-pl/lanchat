using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.Ui
{
    public class LogPanel : SimpleControl
    {
        private readonly VerticalStackPanel stackPanel;

        public LogPanel()
        {
            stackPanel = new VerticalStackPanel();
            Content = stackPanel;
        }

        public void Add(string text)
        {
            foreach (var line in Prepare(text))
            {
                stackPanel.Add(new WrapPanel
                {
                    Content = new HorizontalStackPanel
                    {
                        Children = new[]
                        {
                            new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                            new TextBlock {Text = "-", Color = ConsoleColor.Blue},
                            new TextBlock {Text = "!"},
                            new TextBlock {Text = "- ", Color = ConsoleColor.Blue},
                            new TextBlock {Text = line}
                        }
                    }
                });
            }
        }

        public void AddMessage(string text, string nickname = null)
        {
            foreach (var line in Prepare(text))
            {
                stackPanel.Add(new WrapPanel
                {
                    Content = new HorizontalStackPanel
                    {
                        Children = new[]
                        {
                            new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                            new TextBlock {Text = "<", Color = ConsoleColor.DarkGray},
                            new TextBlock {Text = $"{nickname}"},
                            new TextBlock {Text = "> ", Color = ConsoleColor.DarkGray},
                            new TextBlock {Text = line}
                        }
                    }
                });
            }
        }

        private static IEnumerable<string> Prepare(string text)
        {
            return text.Split(
                new[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None
            );
        }
    }
}