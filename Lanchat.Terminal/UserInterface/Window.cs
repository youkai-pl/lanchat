using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using ConsoleGUI;
using ConsoleGUI.Api;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public class Window
    {
        private readonly DockPanel dockPanel;
        private readonly List<IInputListener> inputListeners = new();
        public TabPanel TabPanel { get; }

        public Window()
        {
            TabPanel = new TabPanel(inputListeners);
            TabsManager = new TabsManager(TabPanel);

            var promptInput = new TextBox();


            var promptBox = new Border
            {
                BorderStyle = BorderStyle.Single,
                Content = new Boundary
                {
                    MinHeight = 1,
                    MaxHeight = 1,
                    Content = new HorizontalStackPanel
                    {
                        Children = new IControl[]
                        {
                            new Style
                            {
                                Content = new PromptIndicator()
                            },
                            promptInput
                        }
                    }
                }
            };

            dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = TabPanel,
                DockedControl = promptBox
            };

            inputListeners.Add(new InputController(promptInput, TabPanel));
            inputListeners.Add(promptInput);
            inputListeners.Add(TabPanel);
        }

        public TabsManager TabsManager { get; }

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
                    ConsoleManager.ReadInput(inputListeners);
                    ConsoleManager.AdjustBufferSize();
                }
                // ReSharper disable once FunctionNeverReturns
            }).Start();
        }
    }
}