using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Figgle;
using Lanchat.Common.NetworkLib;
using System;
using System.Reflection;
using System.Threading;

namespace Lanchat.Terminal.Ui
{
    public static class Prompt
    {
        internal static IInputListener[] InputListener;
        internal static LogPanel Log;
        internal static TextBlock Status = new TextBlock();
        internal static TextBlock Nodes = new TextBlock();
        internal static TextBlock PromptIndicator = new TextBlock();

        public enum OutputType
        {
            Clear,
            Message,
            PrivateMessage,
            Alert,
            Notify
        }

        public static void Start(Config config, Network network)
        {
            // Layout
            Log = new LogPanel();
            var input = new TextBox();

            var dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,

                // Log and prompt
                FillingControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Bottom,
                    DockedControl = new Boundary
                    {
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
                    FillingControl = new Box
                    {
                        VerticalContentPlacement = Box.VerticalPlacement.Bottom,
                        HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
                        Content = Log
                    }
                },

                DockedControl = new Background
                {
                    Color = ConsoleColor.DarkBlue,
                    Content = new Boundary
                    {
                        MinHeight = 1,
                        MaxHeight = 1,
                        Content = new Background
                        {
                            Color = ConsoleColor.DarkBlue,
                            Content = new HorizontalStackPanel
                            {
                                Children = new IControl[] {
                                    Status,
                                    new VerticalSeparator(),
                                    Nodes
                                }
                            }
                        }
                    }
                }
            };

            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            Status.Text = Properties.Resources.Status_Waiting;
            Nodes.Text = Properties.Resources.Status_Waiting;
            PromptIndicator.Text = Properties.Resources.PromptIndicator_Default;
            ConsoleManager.Content = dockPanel;
            InputListener = new IInputListener[]
            {
                new InputController(input, Log, config, network),
                input
            };

            // Write hello screen
            Log.Add(FiggleFonts.Standard.Render(Properties.Resources.Title), OutputType.Clear);
            Log.Add(Assembly.GetExecutingAssembly().GetName().Version.ToString(), OutputType.Clear);

            // Read input
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    ConsoleManager.ReadInput(Prompt.InputListener);
                    ConsoleManager.AdjustBufferSize();
                }
            }).Start();
        }
    }
}