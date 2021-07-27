using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Lanchat.Core.Extensions;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Commands
{
    public class CommandsController
    {
        private readonly TabPanel tabPanel;
        private readonly List<ICommand> commands = new();

        public CommandsController(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
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
                Window.Writer.WriteText(Resources._InvalidCommand);
                return;
            }

            if (args.Length < command.ArgsCount)
            {
                var help = Resources.ResourceManager.GetString($"Help_{commandAlias}", CultureInfo.CurrentCulture);
                if (help != null)
                {
                    Window.Writer.WriteText(help);
                }

                return;
            }


            if (tabPanel.CurrentTab.Content is not ChatView view)
            {
                command.Execute(args);
            }
            else if (view.Node == null)
            {
                command.Execute(args);
            }
            else
            {
                command.Execute(args, view.Node);
            }
        }
    }
}