using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConsoleGUI.Data;
using Lanchat.Terminal.Commands.Status;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.Commands
{
    public class CommandsController
    {
        private readonly TabPanel tabPanel;
        private readonly List<ICommand> commands = new();

        public CommandsController(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            commands.Add(new Nick());
            commands.Add(new Online());
            commands.Add(new Dnd());
            commands.Add(new Afk());
        }

        public void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0][1..];
            args = args.Skip(1).ToArray();
            var command = commands.FirstOrDefault(x => x.Alias == commandAlias);

            if (command == null)
            {
                WriteText(Resources._InvalidCommand);
                return;
            }

            if (args.Length < command.ArgsCount)
            {
                var help = Resources.ResourceManager.GetString($"Help_{commandAlias}", CultureInfo.CurrentCulture);
                if (help != null)
                {
                    WriteText(help);
                }

                return;
            }

            command.Execute(args);
        }

        private void WriteText(string text)
        {
            var writeable = tabPanel.CurrentTab.Content as IWriteable;
            writeable?.AddText(text, Color.White);
        }
    }
}