using System;
using System.Collections.Generic;
using System.Linq;
using Lanchat.Terminal.Commands.Blocking;
using Lanchat.Terminal.Commands.FileTransfer;
using Lanchat.Terminal.Commands.General;
using Lanchat.Terminal.Commands.Status;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Commands
{
    public class CommandsManager
    {
        private readonly List<ICommand> commands = new();

        public CommandsManager()
        {
            // General commands
            commands.Add(new Connect());
            commands.Add(new Disconnect());
            commands.Add(new Exit());
            commands.Add(new Help());
            commands.Add(new Nick());

            // Status commands
            commands.Add(new Afk());
            commands.Add(new Dnd());
            commands.Add(new Online());

            // Blocking commands
            commands.Add(new Block());
            commands.Add(new Blocked());
            commands.Add(new Unblock());

            // File transfer
            commands.Add(new Accept());
            commands.Add(new Cancel());
            commands.Add(new Reject());
            commands.Add(new Send());
        }

        public ICommand GetCommandByAlias(string alias)
        {
            return commands.Find(x => x.Aliases.Contains(alias));
        }

        public void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0][1..];
            args = args.Skip(1).ToArray();
            var command = GetCommandByAlias(commandAlias);
            if (command == null)
            {
                Writer.WriteError(Resources.CommandNotFound);
                return;
            }

            CheckContext(args, command);
        }

        private static void CheckContext(string[] args, ICommand command)
        {
            if (Window.TabPanel.CurrentTab.Content is not ChatView view || view.Node == null)
            {
                if (CheckArgumentsCount(args, command.ArgsCount))
                {
                    command.Execute(args);
                }
            }
            else
            {
                if (CheckArgumentsCount(args, command.ContextArgsCount))
                {
                    command.Execute(args, view.Node);
                }
            }
        }

        private static bool CheckArgumentsCount(IReadOnlyCollection<string> args, int expectedCount)
        {
            if (args.Count >= expectedCount)
            {
                return true;
            }

            Writer.WriteError(Resources.IncorrectCommandUsage);
            return false;
        }
    }
}