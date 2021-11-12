using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Commands
{
    public class CommandsController
    {

        public void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0][1..];
            args = args.Skip(1).ToArray();
            var command = Program.Commands.FirstOrDefault(x => x.Aliases.Contains(commandAlias));
            if (command == null)
            {
                Writer.WriteError(Resources.InvalidCommand);
                return;
            }

            CheckContext(args, command);
        }

        private static void CheckContext(string[] args, ICommand command)
        {
            if (Window.TabPanel.CurrentTab.Content is not ChatView view || view.Node == null)
            {
                if (CheckArgumentsCount(args, command.ArgsCount, command.Aliases[0]))
                {
                    command.Execute(args);
                }
            }
            else
            {
                if (CheckArgumentsCount(args, command.ContextArgsCount, command.Aliases[0]))
                {
                    command.Execute(args, view.Node);
                }
            }
        }

        private static bool CheckArgumentsCount(IReadOnlyCollection<string> args, int expectedCount, string alias)
        {
            if (args.Count >= expectedCount)
            {
                return true;
            }

            var help = Resources.ResourceManager.GetString($"Help_{alias}", CultureInfo.CurrentCulture);
            if (help != null)
            {
                Writer.WriteText(help);
            }

            return false;
        }
    }
}