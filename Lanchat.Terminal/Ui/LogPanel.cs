using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using System;
using System.Globalization;

namespace Lanchat.Terminal.Ui
{
    internal class LogPanel : SimpleControl
    {
        private readonly VerticalStackPanel _stackPanel;

        public LogPanel()
        {
            _stackPanel = new VerticalStackPanel();

            Content = _stackPanel;
        }

        public void Add(string text, Prompt.OutputType outputType, string nickname = null)
        {
            string[] lines = text.Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None
            );

            // Clear output
            if (outputType == Prompt.OutputType.Clear)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = line}
                            }
                        }
                    });
                }
            }

            // Message output
            else if (outputType == Prompt.OutputType.Message)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                        {
                                new TextBlock {Text = $"{DateTime.Now.ToString("HH:mm:ss", CultureInfo.CurrentCulture)} ", Color=ConsoleColor.DarkGray},
                                new TextBlock {Text = $"{nickname}: ", Color=ConsoleColor.DarkCyan},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                }
            }

            // Alert
            if (outputType == Prompt.OutputType.Alert)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"[!] {line}", Color = ConsoleColor.Red}
                            }
                        }
                    });
                }
            }

            // Notice
            if (outputType == Prompt.OutputType.Notify)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"[#] {line}", Color = ConsoleColor.Green}
                            }
                        }
                    });
                }
            }
        }
    }
}