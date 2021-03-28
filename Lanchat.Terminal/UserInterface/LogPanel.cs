using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class LogPanel : SimpleControl
    {
        private readonly object lockUi = new();
        private readonly VerticalStackPanel stackPanel;

        public LogPanel()
        {
            stackPanel = new VerticalStackPanel();
            Content = stackPanel;
        }

        public void Add(string text, ConsoleColor color = ConsoleColor.White)
        {
            foreach (var line in Prepare(text))
            {
                AddText(new[]
                {
                    new TextBlock {Text = line, Color = color}
                });
            }
        }

        public void AddError(string text)
        {
            Add(text, ConsoleColor.Red);
        }
        
        public void AddWarning(string text)
        {
            Add(text, ConsoleColor.Yellow);
        }

        public void AddMessage(string text, string nickname, bool privateMessage)
        {
            var color = privateMessage ? ConsoleColor.Magenta : ConsoleColor.White;
            foreach (var line in Prepare(text))
            {
                AddToLog(new[]
                {
                    new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                    new TextBlock {Text = "<", Color = ConsoleColor.DarkGray},
                    new TextBlock {Text = $"{nickname}", Color = color},
                    new TextBlock {Text = "> ", Color = ConsoleColor.DarkGray},
                    new TextBlock {Text = line}
                });

                Ui.ScrollPanel.Top = int.MaxValue;
            }
        }

        public void AddText(IEnumerable<TextBlock> line)
        {
            var children = new[]
            {
                new TextBlock {Text = $"{DateTime.Now:HH:mm} "},
                new TextBlock {Text = "-", Color = ConsoleColor.Blue},
                new TextBlock {Text = "!"},
                new TextBlock {Text = "- ", Color = ConsoleColor.Blue}
            };

            AddToLog(children.Concat(line));
            Ui.ScrollPanel.Top = int.MaxValue;
        }

        private static IEnumerable<string> Prepare(string text)
        {
            if (text == null) return new[] {""};

            return text.Split(
                new[] {"\r\n", "\r", "\n"},
                StringSplitOptions.None
            );
        }

        private void AddToLog(IEnumerable<TextBlock> textBlocks)
        {
            lock (lockUi)
            {
                stackPanel.Add(new WrapPanel
                {
                    Content = new HorizontalStackPanel
                    {
                        Children = textBlocks
                    }
                });
            }
        }
    }
}