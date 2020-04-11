using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using System.Threading;

namespace Lanchat.Terminal.Ui
{
    class Prompt
    {
        public static LogPanel mainConsole;
        public static IInputListener[] input;

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

            for (int i = 0; ; i++)
            {
                Thread.Sleep(10);
                ConsoleManager.ReadInput(input);
                ConsoleManager.AdjustBufferSize();
            }
        }

        public static void Write(string message)
        {
            mainConsole.Add(message);
        }
    }
}
