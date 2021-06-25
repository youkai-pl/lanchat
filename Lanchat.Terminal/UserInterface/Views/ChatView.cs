using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class ChatView : Tab
    {
        private readonly object lockThread = new();
        private readonly VerticalStackPanel stackPanel = new();
        private readonly VerticalScrollPanel scrollPanel;

        public bool Broadcast { get; }
        public INode Node { get; }

        public ChatView(string name, bool broadcast, INode node = null) : base( name)
        {
            Node = node;
            Broadcast = broadcast;

            scrollPanel = new VerticalScrollPanel
            {
                Content = stackPanel,
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character(),
                ScrollUpKey = ConsoleKey.PageUp,
                ScrollDownKey = ConsoleKey.PageDown
            };

            Content = new Border
            {
                BorderStyle = BorderStyle.Single,
                Content = scrollPanel
            };
        }

        public void Add(string text, ConsoleColor color = ConsoleColor.White)
        {
            foreach (var line in SplitLines(text))
            {
                AddTextLine(new[]
                {
                    new TextBlock {Text = line, Color = color}
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
                scrollPanel.Top = int.MaxValue;
            }
        }

        public void AddTextLine(IEnumerable<TextBlock> line)
        {
            var children = new List<TextBlock>
            {
                new() {Text = $"{DateTime.Now:HH:mm} "},
                new() {Text = "-", Color = ConsoleColor.Blue},
                new() {Text = "!"},
                new() {Text = "- ", Color = ConsoleColor.Blue}
            };
            children.AddRange(line);
            AddToLog(children);
            scrollPanel.Top = int.MaxValue;
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
            lock (lockThread)
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