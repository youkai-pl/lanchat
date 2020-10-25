using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class Unblock
    {
        public static void Execute(string[] args, Config config)
        {
            if (args == null || args.Length < 1)
            {
                Prompt.Log.Add(Resources.Manual_Unblock);
                return;
            }

            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                config.RemoveBlocked(parsedIp);
                Prompt.Log.Add($"{parsedIp} unblocked");
            }
            else
            {
                Prompt.Log.Add(Resources._IncorrectValues);
            }
        }
    }
}