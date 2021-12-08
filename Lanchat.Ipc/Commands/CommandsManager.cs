using System;
using System.Collections.Generic;
using System.Linq;
using Lanchat.Ipc.Commands.General;

namespace Lanchat.Ipc.Commands
{
    public class CommandsManager
    {
        private readonly List<ICommand> commands = new();

        public CommandsManager()
        {
            // General commands
            commands.Add(new Broadcast());
            commands.Add(new Connect());
            commands.Add(new Disconnect());
            commands.Add(new Nick());
        }

        public ICommand GetCommandByAlias(string alias)
        {
            return commands.Find(x => x.Alias == alias);
        }

        public void ExecuteCommand(string commandString)
        {
            var args = commandString.Split("/");
            var commandAlias = args[0];
            args = args.Skip(1).ToArray();
            var command = GetCommandByAlias(commandAlias);
            if (command == null)
            {
                Program.IpcSocket.Send("invalid_command");
                return;
            }

            CheckArgumentsCount(args, command.ArgsCount);
            command.Execute(args);
        }

        private static bool CheckArgumentsCount(IReadOnlyCollection<string> args, int expectedCount)
        {
            if (args.Count >= expectedCount)
            {
                return true;
            }

            Program.IpcSocket.Send("invalid_arguments");
            return false;
        }
    }
}