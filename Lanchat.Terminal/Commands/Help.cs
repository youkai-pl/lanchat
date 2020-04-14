using Lanchat.Terminal.Ui;
using System;
using System.Globalization;

namespace Lanchat.Terminal.Commands
{
    public static class Help
    {
        public static void Execute(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length < 1)
            {
                Prompt.Log.Add(Properties.Resources.Manual_Help);
            }
            else
            {
                var command = $"Manual_{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(args[0].ToLower(CultureInfo.CurrentCulture))}";
                var commandHelp = Properties.Resources.ResourceManager.GetObject(command, Properties.Resources.Culture);
                if(commandHelp != null)
                {
                    Prompt.Log.Add(commandHelp.ToString());
                }
                else
                {
                    Prompt.Log.Add(Properties.Resources._ManualNotFound);
                }
            }
        }

        public static string Man { get; private set; } = Properties.Resources.Manual_Help;
    }
}
