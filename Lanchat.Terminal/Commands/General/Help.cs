using System.Globalization;
using Lanchat.Terminal.Properties;

namespace Lanchat.Terminal.Commands.General
{
    public class Help : ICommand
    {
        public string Alias => "help";
        public int ArgsCount => 0;

        public void Execute(string[] args)
        {
            var tabsManager = Program.Window.TabsManager;

            if (args.Length < 1)
            {
                tabsManager.WriteText(Resources.Help);
            }
            else
            {
                var commandHelp = Resources.ResourceManager.GetString($"Help_{args[0]}", CultureInfo.CurrentCulture);
                tabsManager.WriteError(commandHelp ?? Resources._ManualNotFound);
            }
        }
    }
}