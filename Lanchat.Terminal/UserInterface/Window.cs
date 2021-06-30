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

namespace Lanchat.Terminal.UserInterface
{
    public class Window
    {
        private readonly DockPanel dockPanel;
        private readonly List<IInputListener> inputListeners = new();

        public Window()
        {
            var tabPanel = new TabPanel(inputListeners);
            TabsManager = new TabsManager(tabPanel);

            var promptInput = new TextBox();
            var promptIndicator = new TextBlock
            {
                Text = $"[{Program.Config.Nickname} (Online)] "
            };

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
                                Content = promptIndicator
                            },
                            promptInput
                        }
                    }
                }
            };

            dockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = tabPanel,
                DockedControl = promptBox
            };

            inputListeners.Add(new InputController(promptInput, tabPanel));
            inputListeners.Add(promptInput);
            inputListeners.Add(tabPanel);
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