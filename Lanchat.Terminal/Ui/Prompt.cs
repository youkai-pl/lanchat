using System;
using System.Globalization;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Lanchat.Core;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Ui
{
    public static class Prompt
    {
        private static IInputListener[] _inputListener;
        internal static LogPanel Log;
        private static TextBlock _clock;
        private static TextBlock _port;
        private static TextBlock _nodes;
        private static TextBlock _promptIndicator;
        private static InputController _inputController;

        public static void Start(Config config, P2P network)
        {
            // Layout
            Log = new LogPanel();
            var input = new TextBox();
            var topBar = new TextBlock();
            _inputController = new InputController(input, Log, config, network);

            topBar.Text = $" {Resources.Title} - {Resources.PageLink}";

            _clock = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                Color = ConsoleColor.Gray
            };

            _nodes = new TextBlock
            {
                Color = ConsoleColor.Gray
            };

            _port = new TextBlock
            {
                Color = ConsoleColor.Gray
            };

            if (config != null)
            {
                _promptIndicator = new TextBlock
                {
                    Text = $"[{config.Nickname}]> "
                };
            }
            else
            {
                _promptIndicator = new TextBlock
                {
                    Text = Resources.PromptIndicator_Default
                };
            }

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
                            Content = topBar
                        }
                    },

                    // Log
                    FillingControl = new Box
                    {
                        VerticalContentPlacement = Box.VerticalPlacement.Bottom,
                        HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
                        Content = Log
                    }
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
                                    Content = _promptIndicator
                                },
                                input
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
                                    new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                                    _clock,
                                    new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                                    _port,
                                    new TextBlock {Text = "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock {Text = " [", Color = ConsoleColor.DarkCyan},
                                    _nodes,
                                    new TextBlock {Text = "] ", Color = ConsoleColor.DarkCyan}
                                }
                            }
                        }
                    }
                }
            };

            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            Console.Title = Resources.Title;
            ConsoleManager.Resize(new Size(100, 30));

            ConsoleManager.Content = dockPanel;
            _inputListener = new IInputListener[]
            {
                _inputController,
                input
            };

            Log.Add(Resources.HelloAsci);

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    _clock.Text = DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture);
                    ConsoleManager.ReadInput(_inputListener);
                    ConsoleManager.AdjustBufferSize();
                }
            }).Start();
        }
    }
}