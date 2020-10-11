using System;
using System.Globalization;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

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
                Prompt.Log.Add(Resources.Manual_Help);
            }
            else
            {
                var command = $"Manual_{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(args[0].ToLower())}";
                var commandHelp = Resources.ResourceManager.GetObject(command);
                Prompt.Log.Add(commandHelp != null ? commandHelp.ToString() : Resources._ManualNotFound);
            }
        }
    }
}