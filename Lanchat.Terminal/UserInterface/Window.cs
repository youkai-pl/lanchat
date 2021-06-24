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
        private readonly TabPanel tabPanel;
        private readonly DockPanel dockPanel;
        private readonly TextBox promptInput;
        private readonly BottomBar bottomBar;
        private readonly List<IInputListener> inputListeners = new();

        public TabsManager TabsManager { get; }

        public Window()
        {
            tabPanel = new TabPanel(inputListeners);
            TabsManager = new TabsManager(tabPanel);

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
                FillingControl = tabPanel,
                DockedControl = new DockPanel
                {
                    Placement = DockPanel.DockedControlPlacement.Bottom,
                    DockedControl = promptBox,
                    FillingControl = bottomBar
                }
            };

            inputListeners.Add(new InputController(promptInput, tabPanel));
            inputListeners.Add(promptInput);
            inputListeners.Add(tabPanel);
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

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    bottomBar.Clock.Text = DateTime.Now.ToString("HH:mm");
                    ConsoleManager.ReadInput(inputListeners);
                    ConsoleManager.AdjustBufferSize();
                }
            }).Start();

            // ReSharper disable once FunctionNeverReturns
        }
    }
}