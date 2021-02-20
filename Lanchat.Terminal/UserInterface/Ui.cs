using System;
using System.Reflection;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Lanchat.ClientCore;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public static class Ui
    {
        private static TextBlock _clock;
        private static TextBox _input;
        internal static LogPanel Log { get; private set; }
        internal static TextBlock NodesCount { get; private set; }
        internal static TextBlock DetectedCount { get; private set; }
        internal static TextBlock PromptIndicator { get; private set; }
        internal static VerticalScrollPanel ScrollPanel { get; private set; }
        internal static TextBlock StatusBar { get; private set; }
        internal static TextBlock Status { get; private set; }

        internal static TextBlock FileTransferStatus { get; private set; }

        internal static FileTransferProgressMonitor FileTransferProgressMonitor { get; } = new();

        public static void Start()
        {
            Log = new LogPanel();

            _input = new TextBox();

            _clock = new TextBlock
            {
                Color = ConsoleColor.Gray
            };

            Status = new TextBlock
            {
                Text = "Online",
                Color = ConsoleColor.Gray
            };

            NodesCount = new TextBlock
            {
                Text = "0",
                Color = ConsoleColor.Gray
            };

            DetectedCount = new TextBlock
            {
                Text = "0",
                Color = ConsoleColor.Gray
            };

            PromptIndicator = new TextBlock
            {
                Text = $"[{Program.Config.Nickname}] "
            };

            FileTransferStatus = new TextBlock
            {
                Text = FileTransferProgressMonitor.Text,
                Color = ConsoleColor.Gray
            };

            ScrollPanel = new VerticalScrollPanel
            {
                Content = Log,
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character()
            };

            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString();
            version = version?.Remove(version.Length - 2);

            StatusBar = new TextBlock
            {
                Text = $"Lanchat {version}"
            };

            var dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Top,

                    // Top bar
                    DockedControl = new Boundary
                    {
                        MaxHeight = 1,
                        Content = new Background
                        {
                            Color = ConsoleColor.DarkBlue,
                            Content = StatusBar
                        }
                    },

                    // Log
                    FillingControl = ScrollPanel
                },

                DockedControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Bottom,

                    // Prompt
                    DockedControl = new Boundary
                    {
                        MinHeight = 1,
                        MaxHeight = 1,
                        Content = new HorizontalStackPanel
                        {
                            Children = new IControl[]
                            {
                                new Style
                                {
                                    Content = PromptIndicator
                                },
                                _input
                            }
                        }
                    },

                    // Bottom bar
                    FillingControl = new Boundary
                    {
                        MaxHeight = 1,
                        Content = new Background
                        {
                            Color = ConsoleColor.DarkBlue,
                            Content = new HorizontalStackPanel
                            {
                                Children = new IControl[]
                                {
                                    new TextBlock {Text = "[", Color = ConsoleColor.DarkCyan},
                                    _clock,
                                    new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                                    NodesCount,
                                    new TextBlock {Text = "/"},
                                    DetectedCount,
                                    new TextBlock {Text = "] ", Color = ConsoleColor.DarkCyan},
                                    new TextBlock {Text = "[", Color = ConsoleColor.DarkCyan},
                                    Status,
                                    new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock {Text = "[", Color = ConsoleColor.DarkCyan},
                                    FileTransferStatus,
                                    new TextBlock {Text = "] ", Color = ConsoleColor.DarkCyan}
                                }
                            }
                        }
                    }
                }
            };

            FileTransferProgressMonitor.PropertyChanged += (_, _) =>
            {
                FileTransferStatus.Text = FileTransferProgressMonitor.Text;
            };
            
            // Start console UI 
            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            ConsoleManager.Content = dockPanel;
            Console.Title = Resources._WindowTitle;
            Log.Add(Resources._Logo);

            if (Program.Config.Fresh) Log.Add(string.Format(Resources._FirstRunMessage, Config.ConfigPath));

            // Clock updates
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    _clock.Text = DateTime.Now.ToString("HH:mm");
                    ConsoleManager.ReadInput(new IInputListener[]
                    {
                        new InputController(_input),
                        ScrollPanel,
                        _input
                    });
                    ConsoleManager.AdjustBufferSize();
                }

                // ReSharper disable once FunctionNeverReturns
            }).Start();
        }
    }
}