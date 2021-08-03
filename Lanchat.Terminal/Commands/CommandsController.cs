using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Commands
{
    public class CommandsController
    {
        private readonly List<ICommand> commands = new();

        public CommandsController()
        {
            var ass = Assembly.GetEntryAssembly();
            ass?.DefinedTypes.ForEach(x =>
            {
                if (x.ImplementedInterfaces.Contains(typeof(ICommand)))
                {
                    commands.Add(ass.CreateInstance(x.FullName!) as ICommand);
                }
            });
        }

        public void ExecuteCommand(string[] args)
        {
            var commandAlias = args[0][1..];
            args = args.Skip(1).ToArray();
            var command = commands.FirstOrDefault(x => x.Alias == commandAlias);

            if (command == null)
            {
                Writer.WriteText(Resources._InvalidCommand);
                return;
            }


            if (Window.TabPanel.CurrentTab.Content is not ChatView view || view.Node == null)
            {
                if (CheckArgumentsCount(args, command.ArgsCount, commandAlias))
                {
                    command.Execute(args);
                }
            }
            else
            {
                if (CheckArgumentsCount(args, command.ContextArgsCount, commandAlias))
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