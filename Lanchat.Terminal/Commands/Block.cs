using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class Block
    {
        public static void Execute(string[] args, Config config)
        {
            if (args == null || args.Length < 1)
            {
                Prompt.Log.Add(Resources.Manual_Block);
                return;
            }

            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                config.AddBlocked(parsedIp);
                Prompt.Log.Add($"{parsedIp} blocked");
            }
            else
            {
                Prompt.Log.Add(Resources._IncorrectValues);
            }
        }
    }
}