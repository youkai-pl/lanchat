using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using System;
using System.Globalization;

namespace Lanchat.Terminal.Ui
{
    public class LogPanel : SimpleControl
    {
        private readonly VerticalStackPanel _stackPanel;

        public LogPanel()
        {
            _stackPanel = new VerticalStackPanel();
            Content = _stackPanel;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public void Add(string text, Prompt.OutputType outputType = Prompt.OutputType.System, string nickname = null)
        {
            string[] lines;

            if (text != null)
            {
                lines = text.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );
            }
            else
            {
                throw new ArgumentNullException(nameof(text));
            }
        
            // System message
            if (outputType == Prompt.OutputType.System)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"{DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture)} "},
                                new TextBlock {Text = "-", Color=ConsoleColor.Blue},
                                new TextBlock {Text = "!"},
                                new TextBlock {Text = "- ", Color=ConsoleColor.Blue},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                }
            }

            // Message
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
                                new TextBlock {Text = $"{DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture)} "},
                                new TextBlock {Text = "<", Color=ConsoleColor.DarkGray},
                                new TextBlock {Text = $"{nickname}"},
                                new TextBlock {Text = "> ", Color=ConsoleColor.DarkGray},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                }
            }

            // Private message
            else if (outputType == Prompt.OutputType.PrivateMessage)
            {
                foreach (var line in lines)
                {
                    _stackPanel.Add(new WrapPanel
                    {
                        Content = new HorizontalStackPanel
                        {
                            Children = new[]
                            {
                                new TextBlock {Text = $"{DateTime.Now.ToString("HH:mm", CultureInfo.CurrentCulture)} "},
                                new TextBlock {Text = "<", Color=ConsoleColor.DarkGray},
                                new TextBlock {Text = $"# {nickname}"},
                                new TextBlock {Text = "> ", Color=ConsoleColor.DarkGray},
                                new TextBlock {Text = line}
                            }
                        }
                    });
                }
            }
        }
    }
}