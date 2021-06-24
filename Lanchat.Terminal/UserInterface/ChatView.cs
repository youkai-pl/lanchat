using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;

namespace Lanchat.Terminal.UserInterface
{
    public class ChatView : VerticalScrollPanel
    {
        private readonly object lockThread;
        private readonly VerticalStackPanel stackPanel;
        
        public string Id { get; }

        public ChatView(string id)
        {
            Id = id;
            lockThread = new object();
            stackPanel = new VerticalStackPanel();
            
            Content = stackPanel;
            ScrollBarBackground = new Character();
            ScrollBarForeground = new Character();
            ScrollUpKey = ConsoleKey.PageUp;
            ScrollDownKey = ConsoleKey.PageDown;
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

        public void AddError(string text)
        {
            Add(text, ConsoleColor.Red);
        }

        public void AddWarning(string text)
        {
            Add(text, ConsoleColor.Yellow);
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
                Top = int.MaxValue;
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
            Top = int.MaxValue;
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