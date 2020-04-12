using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Figgle;
using System;
using System.Threading;

namespace Lanchat.Terminal.Ui
{
    internal class Prompt
    {
        public static IInputListener[] input;
        public static LogPanel log;
        public static TextBlock status;
        public static TextBlock nodes;

        public enum OutputType
        {
            Clear,
            Message,
            Alert,
            Notify
        }

        public static void Start()
        {
            // Layout
            log = new LogPanel();
            status = new TextBlock();
            nodes = new TextBlock();
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
                                    Content = new TextBlock { Text = Properties.Resources.PromptIndicator }
                                },
                                input
                            }
                        }
                    },
                    FillingControl = new Box
                    {
                        VerticalContentPlacement = Box.VerticalPlacement.Bottom,
                        HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
                        Content = log
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
                                    status,
                                    new VerticalSeparator(),
                                    nodes
                                }
                            }
                        }
                    }
                }
            };

            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            status.Text = Properties.Resources.Status_Waiting;
            nodes.Text = Properties.Resources.Status_Waiting;
            ConsoleManager.Content = dockPanel;
            Prompt.input = new IInputListener[]
            {
                new InputController(input, log),
                input
            };

            // Write hello screen
            log.Add(FiggleFonts.Standard.Render(Properties.Resources.Title), OutputType.Clear);

            // Read input
            for (int i = 0; ; i++)
            {
                Thread.Sleep(10);
                ConsoleManager.ReadInput(Prompt.input);
                ConsoleManager.AdjustBufferSize();
            }
        }
    }
}