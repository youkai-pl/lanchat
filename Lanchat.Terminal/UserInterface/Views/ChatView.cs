using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface.Views
{
    public class ChatView : VerticalStackPanel
    {
        private readonly object lockThread = new();
        public bool Broadcast { get; }
        public INode Node { get; }

        public ChatView(bool broadcast, INode node = null)
        {
            Node = node;
            Broadcast = broadcast;
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
                base.Add(new WrapPanel
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