using System;
using ConsoleGUI;
using ConsoleGUI.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public class BottomBar : Boundary
    {
        public BottomBar()
        {
            Status = new TextBlock
            {
                Text = "Online",
                Color = ConsoleColor.Gray
            };

            Clock = new TextBlock
            {
                Color = ConsoleColor.Gray
            };

            NodesCount = new TextBlock
            {
                Text = "0",
                Color = ConsoleColor.Gray
            };
            
            MaxHeight = 1;
            Content = new Background
            {
                Color = ConsoleColor.Blue,
                Content = new HorizontalStackPanel
                {
                    Children = new IControl[]
                    {
                        new TextBlock {Text = "[", Color = ConsoleColor.DarkBlue},
                        Clock,
                        new TextBlock {Text = "]", Color = ConsoleColor.DarkBlue},
                        new TextBlock {Text = " [", Color = ConsoleColor.DarkBlue},
                        NodesCount,
                        new TextBlock {Text = "] ", Color = ConsoleColor.DarkBlue},
                        new TextBlock {Text = "[", Color = ConsoleColor.DarkBlue},
                        Status,
                        new TextBlock {Text = "] ", Color = ConsoleColor.DarkBlue}
                    }
                }
            };
        }

        public TextBlock Status { get; }
        public TextBlock NodesCount { get; }
        public TextBlock Clock { get; }
    }
}