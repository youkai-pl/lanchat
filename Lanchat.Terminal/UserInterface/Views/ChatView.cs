using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class ChatView : SimpleControl, IScrollable, IWriteable
    {
        private readonly VerticalStackPanel stackPanel = new();

        public ChatView(INode node = null)
        {
            Node = node;
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

        public INode Node { get; }
        public VerticalScrollPanel ScrollPanel { get; }

        public void AddText(string text, Color color)
        {
            foreach (var line in SplitLines(text))
            {
                AddToLog(
                    new List<TextBlock>
                    {
                        new() { Text = $"{DateTime.Now:HH:mm:ss}", Color = Theme.LogHour },
                        new() { Text = " " },
                    },
                    new List<TextBlock>
                    {
                        new() { Text = line, Color = color }
                    });
            }
        }

        public void AddMessage(string text, string nickname)
        {
            foreach (var line in SplitLines(text))
            {
                AddToLog(
                    new List<TextBlock>
                    {
                        new() { Text = $"{DateTime.Now:HH:mm:ss}", Color = Theme.LogHour },
                        new() { Text = " " },
                        new() { Text = nickname, Color = Theme.LogNickname },
                        new() { Text = " " },
                    },
                    new List<TextBlock>
                    {
                        new() { Text = line, Color = Theme.Foreground }
                    });
            }
        }

        private static IEnumerable<string> SplitLines(string text)
        {
            if (text == null)
            {
                return new[]
                {
                    ""
                };
            }

            return text.Split(new[]
                {
                    "\r\n", "\r", "\n"
                },
                StringSplitOptions.None
            );
        }

        private void AddToLog(List<TextBlock> header, List<TextBlock> content)
        {
            Window.UiAction(() =>
            {
                stackPanel.Add(
                    new DockPanel
                    {
                        DockedControl = new WrapPanel
                        {
                            Content = new HorizontalStackPanel
                            {
                                Children = header
                            }
                        },
                        FillingControl = new WrapPanel
                        {
                            Content = new HorizontalStackPanel
                            {
                                Children = content
                            }
                        },
                        Placement = DockPanel.DockedControlPlacement.Left
                    });
                ScrollPanel.Top = int.MaxValue;
            });
        }
    }
}