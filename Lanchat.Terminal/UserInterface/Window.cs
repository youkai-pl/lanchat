using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
        private readonly List<IInputListener> inputListeners = new();

        public Window()
        {
            TabPanel = new TabPanel(inputListeners);
            var c1 = new ChatView();
            var c2 = new ChatView();
            var c3 = new ChatView();
            
            TabPanel.AddTab("Lanchat", new HomeTab());
            TabPanel.AddTab("#main", c1);
            TabPanel.AddTab("@admin#4324", c2);
            TabPanel.AddTab("@user#4324", c3);

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
            
            inputListeners.Add(new InputController(promptInput, TabPanel));
            inputListeners.Add(promptInput);
            inputListeners.Add(TabPanel);
        }

        public void Start()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConsoleManager.Console = new SimplifiedConsole();
            }
            
            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(100, 30));
            ConsoleManager.Content = dockPanel;
            Console.Title = Resources._WindowTitle;

            while (true)
            {
                Thread.Sleep(10);
                bottomBar.Clock.Text = DateTime.Now.ToString("HH:mm");
                ConsoleManager.ReadInput(inputListeners);
                ConsoleManager.AdjustBufferSize();
            }

            // ReSharper disable once FunctionNeverReturns
        }
    }
}