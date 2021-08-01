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
                AddToLog(new List<TextBlock>
                {
                    new() {Text = $"{DateTime.Now:HH:mm} "},
                    new() {Text = line, Color = color}
                });
            }
        }

        public void AddMessage(string text, string nickname)
        {
            foreach (var line in SplitLines(text))
            {
                AddToLog(new List<TextBlock>
                {
                    new() {Text = $"{DateTime.Now:HH:mm} "},
                    new() {Text = "<", Color = ConsoleColor.DarkGray},
                    new() {Text = $"{nickname}"},
                    new() {Text = "> ", Color = ConsoleColor.DarkGray},
                    new() {Text = line, Color = ConsoleColor.White}
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

        private void AddToLog(IEnumerable<TextBlock> textBlocks)
        {
            Window.UiAction(() =>
            {
                stackPanel.Add(new WrapPanel
                {
                    Content = new HorizontalStackPanel
                    {
                        Children = textBlocks
                    }
                });
                ScrollPanel.Top = int.MaxValue;
            });
        }
    }
}