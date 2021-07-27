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
    public static class Window
    {
        private static readonly DockPanel DockPanel;
        private static readonly List<IInputListener> InputListeners = new();
        private static TabPanel TabPanel { get; }
        private static readonly object Locker = new();

        static Window()
        {
            TabPanel = new TabPanel(InputListeners);
            TabsManager = new TabsManager(TabPanel);
            Writer = new Writer(TabPanel);

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

            DockPanel = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Bottom,
                FillingControl = TabPanel,
                DockedControl = promptBox
            };

            InputListeners.Add(new InputController(promptInput, TabPanel));
            InputListeners.Add(promptInput);
            InputListeners.Add(TabPanel);
        }

        public static TabsManager TabsManager { get; }
        public static Writer Writer { get; }

        public static void Start()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ConsoleManager.Console = new SimplifiedConsole();
            }

            ConsoleManager.Setup();
            ConsoleManager.Resize(new Size(140, 30));
            ConsoleManager.Content = DockPanel;
            Console.Title = Resources._WindowTitle;

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(10);
                    UiAction(() =>
                    {
                        ConsoleManager.ReadInput(InputListeners);
                        ConsoleManager.AdjustBufferSize();
                    });
                }
                // ReSharper disable once FunctionNeverReturns
            }).Start();
        }

        public static void UiAction(Action action)
        {
            lock (Locker)
            {
                action();
            }
        }
    }
}