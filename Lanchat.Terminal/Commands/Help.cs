using System.Globalization;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Help : ICommand
    {
        public string Alias { get; set; } = "help";
        public int ArgsCount { get; set; }

        public void Execute(string[] args)
        {
            if (args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Help);
            }
            else
            {
                var command = $"Manual_{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(args[0].ToLower())}";
                var commandHelp = Resources.ResourceManager.GetObject(command);
                Ui.Log.Add(commandHelp != null ? commandHelp.ToString() : Resources.Info_ManualNotFound);
            }
        }
    }
}