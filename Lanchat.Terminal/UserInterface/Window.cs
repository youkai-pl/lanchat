using System;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.UserInterface
{
    public class Window
    {
        public TabPanel TabPanel { get; }
        private readonly DockPanel dockPanel;
        private readonly TextBox promptInput;
        private readonly BottomBar bottomBar;

        public Window()
        {
            TabPanel = new TabPanel();
            TabPanel.AddTab("#main", new ChatView());
            TabPanel.AddTab("@admin#4324", new ChatView());
            TabPanel.AddTab("@user#4324", new ChatView());

            promptInput = new TextBox();
            var promptIndicator = new TextBlock
            {
                Text = $"[{Program.Config.Nickname}] "
            };

            var promptBox = new Boundary
            {
                MinHeight = 1,
                MaxHeight = 1,
                Content = new HorizontalStackPanel
                {
                    Children = new IControl[]
                    {
                        new Style
                        {
                            Content = promptIndicator
                        },
                        promptInput
                    }
                }
            };

            bottomBar = new BottomBar();

            dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = TabPanel,

                DockedControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Bottom,
                    DockedControl = promptBox,
                    FillingControl = bottomBar
                }
            };
        }

        public void Start()
        {
            ConsoleManager.Console = new SimplifiedConsole();
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            ConsoleManager.Content = dockPanel;
            Console.Title = Resources._WindowTitle;

            var input = new IInputListener[]
            {
                new InputController(promptInput, TabPanel),
                promptInput,
                TabPanel
            };

            while (true)
            {
                Thread.Sleep(10);
                bottomBar.Clock.Text = DateTime.Now.ToString("HH:mm");
                ConsoleManager.ReadInput(input);
                ConsoleManager.AdjustBufferSize();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}