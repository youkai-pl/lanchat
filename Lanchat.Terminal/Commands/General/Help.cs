using System.Globalization;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Help : ICommand
    {
        public string[] Aliases { get; } =
        {
            "help",
            "h"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Writer.WriteText(string.Format(Resources.Help, Paths.ConfigFile));
            }
            else
            {
                var command = Program.CommandsManager.GetCommandByAlias(args[0]);
                if (command == null)
                {
                    Writer.WriteError(Resources.CommandNotFound);
                    return;
                }

                var aliases = string.Join(", ", command.Aliases);
                var commandSyntax = Resources.ResourceManager.GetString($"Syntax_{command.Aliases[0]}", CultureInfo.CurrentCulture);
                var commandSummary = Resources.ResourceManager.GetString($"Summary_{command.Aliases[0]}", CultureInfo.CurrentCulture);

                Writer.WriteText("");
                if (commandSummary != null)
                {
                    Writer.WriteStatus(Resources.Summary);
                    Writer.WriteText(commandSummary);
                }
                if (commandSyntax != null)
                {
                    Writer.WriteStatus(Resources.Syntax);
                    Writer.WriteText(commandSyntax);
                }
                Writer.WriteStatus(Resources.Aliases);
                Writer.WriteText("");
                Writer.WriteText($"    {aliases}");
                Writer.WriteText("");
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}