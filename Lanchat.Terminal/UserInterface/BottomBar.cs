using System;
using ConsoleGUI;
using ConsoleGUI.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public class BottomBar : Boundary
    {
        private readonly TextBlock detectedCount;
        private readonly TextBlock fileTransferStatus;

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

            detectedCount = new TextBlock
            {
                Text = "0",
                Color = ConsoleColor.Gray
            };

            fileTransferStatus = new TextBlock
            {
                Text = FileTransferProgressMonitor.Text,
                Color = ConsoleColor.Gray
            };

            MaxHeight = 1;
            Content = new Background
            {
                Color = ConsoleColor.DarkBlue,
                Content = new HorizontalStackPanel
                {
                    Children = new IControl[]
                    {
                        new TextBlock {Text = "[", Color = ConsoleColor.DarkCyan},
                        Clock,
                        new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                        new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                        NodesCount,
                        new TextBlock {Text = "/"},
                        detectedCount,
                        new TextBlock {Text = "] ", Color = ConsoleColor.DarkCyan},
                        new TextBlock {Text = "[", Color = ConsoleColor.DarkCyan},
                        Status,
                        new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                        new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                        fileTransferStatus,
                        new TextBlock {Text = "] ", Color = ConsoleColor.DarkCyan}
                    }
                }
            };
        }

        internal TextBlock Status { get; }
        internal FileTransferProgressMonitor FileTransferProgressMonitor { get; } = new();
        internal TextBlock NodesCount { get; }
        internal TextBlock Clock { get; }

        internal void SetupEvents()
        {
            FileTransferProgressMonitor.PropertyChanged += (_, _) =>
            {
                fileTransferStatus.Text = FileTransferProgressMonitor.Text;
            };

            Program.Network.Broadcasting.DetectedNodes.CollectionChanged += (_, _) =>
            {
                detectedCount.Text = Program.Network.Broadcasting.DetectedNodes.Count.ToString();
            };
        }
    }
}