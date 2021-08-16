using System.Globalization;
using Lanchat.ClientCore;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.General
{
    public class Help : ICommand
    {
        public string Alias => "help";
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Writer.WriteText(string.Format(Resources.Help, Storage.ConfigPath));
            }
            else
            {
                var commandHelp = Resources.ResourceManager.GetString($"Help_{args[0]}", CultureInfo.CurrentCulture);
                if (commandHelp == null)
                {
                    Writer.WriteError(Resources._ManualNotFound);
                }
                else
                {
                    Writer.WriteText(commandHelp);
                }
            }
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}