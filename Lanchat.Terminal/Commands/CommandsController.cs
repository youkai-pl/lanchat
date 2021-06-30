using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands
{
    public class CommandsController
    {
        private readonly List<ICommand> commands = new();

        public CommandsController()
        {
            commands.Add(new Nick());
        }
        
        public void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0][1..];
            args = args.Skip(1).ToArray();
            var command = commands.FirstOrDefault(x => x.Alias == commandAlias);

            if (command == null)
            {
                // TODO: Show error
                return;
            }

            if (args.Length < command.ArgsCount)
            {
                var help = Resources.ResourceManager.GetString($"Help_{commandAlias}", CultureInfo.CurrentCulture);
                if (help != null)
                {
                    // TODO: Show help
                }

                return;
            }

            command.Execute(args);
        }
    }
}