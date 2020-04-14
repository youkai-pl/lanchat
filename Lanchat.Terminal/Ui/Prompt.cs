using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Figgle;
using Lanchat.Common.NetworkLib;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace Lanchat.Terminal.Ui
{
    public static class Prompt
    {
        internal static IInputListener[] InputListener;
        internal static LogPanel Log;
        internal static TextBlock Clock;
        internal static TextBlock Port;
        internal static TextBlock Nodes;
        internal static TextBlock PromptIndicator;

        public enum OutputType
        {
            System,
            Message,
            PrivateMessage,
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public static void Start(Config config, Network network)
        {
            // Layout
            Log = new LogPanel();
            var input = new TextBox();
            var topBar = new TextBlock();

            topBar.Text = $" {Properties.Resources.Title} {Assembly.GetExecutingAssembly().GetName().Version} - {Properties.Resources.PageLink}";

            Clock = new TextBlock()
            {
                Text = DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture),
                Color = ConsoleColor.Gray
            };

            Nodes = new TextBlock()
            {
                Text = Properties.Resources.Status_Waiting,
                Color = ConsoleColor.Gray
            };

            Port = new TextBlock()
            {
                Text = Properties.Resources.Status_Waiting,
                Color = ConsoleColor.Gray
            };

            if (config != null)
            {
                PromptIndicator = new TextBlock
                {
                    Text = $"[{config.Nickname}]> "
                };
            }
            else
            {
                PromptIndicator = new TextBlock
                {
                    Text = Properties.Resources.PromptIndicator_Default
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
                                    Content = PromptIndicator
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
                                Children = new IControl[] {
                                    new TextBlock(){Text= " [", Color = ConsoleColor.DarkCyan},
                                    Clock,
                                    new TextBlock(){Text= "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock(){Text= " [", Color = ConsoleColor.DarkCyan},
                                    Port,
                                    new TextBlock(){Text= "]", Color = ConsoleColor.DarkCyan},
                                    new TextBlock(){Text= " [", Color = ConsoleColor.DarkCyan},
                                    Nodes,
                                    new TextBlock(){Text= "] ", Color = ConsoleColor.DarkCyan},
                                }
                            }
                        }
                    }
                }
            };

            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            Console.Title = "Lanchat 2";
            ConsoleManager.Resize(new Size(100, 30));

            ConsoleManager.Content = dockPanel;
            InputListener = new IInputListener[]
            {
                new InputController(input, Log, config, network),
                input
            };

            Log.Add(FiggleFonts.Standard.Render(Properties.Resources.Title), OutputType.System);

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    Clock.Text = DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture);
                    ConsoleManager.ReadInput(InputListener);
                    ConsoleManager.AdjustBufferSize();
                }
            }).Start();
        }
    }
}