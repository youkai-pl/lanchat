using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Figgle;
using Lanchat.Common.Types;
using System;
using System.Globalization;
using System.Threading;

namespace Lanchat.Terminal.Ui
{
    internal class Prompt
    {
        public static IInputListener[] input;
        public static LogPanel mainConsole;

        public enum OutputType
        {
            Clear,
            Message,
            Alert,
            Notify
        }

        public static void Start()
        {
            var textBox = new TextBox();
            mainConsole = new LogPanel();
            var dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Top,
                FillingControl = new DockPanel
                {
                    FillingControl = new Overlay
                    {
                        BottomContent = new DockPanel
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
                                        textBox
                                    }
                                }
                            },
                            FillingControl = new Box
                            {
                                VerticalContentPlacement = Box.VerticalPlacement.Bottom,
                                HorizontalContentPlacement = Box.HorizontalPlacement.Stretch,
                                Content = mainConsole
                            }
                        }
                    }
                }
            };

            ConsoleManager.Setup();
            ConsoleManager.Content = dockPanel;
            input = new IInputListener[]
            {
                new InputController(textBox, mainConsole),
                textBox
            };

            // Write hello screen
            mainConsole.Add(FiggleFonts.Standard.Render(Properties.Resources.Title), OutputType.Clear);

            // Test outputs
            mainConsole.Add(Properties.Resources.Title, OutputType.Clear);
            mainConsole.Add(Properties.Resources.Title, OutputType.Notify);
            mainConsole.Add(Properties.Resources.Title, OutputType.Alert);
            mainConsole.Add(Properties.Resources.Title, OutputType.Message, "test");

            mainConsole.Add("", OutputType.Clear);


            // Read input
            for (int i = 0; ; i++)
            {
                Thread.Sleep(10);
                ConsoleManager.ReadInput(input);
                ConsoleManager.AdjustBufferSize();
            }
        }
    }
}