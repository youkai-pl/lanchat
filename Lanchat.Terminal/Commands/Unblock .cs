using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Unblock
    {
        public static void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Unblock);
                return;
            }

            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                Program.Config.RemoveBlocked(parsedIp);
                Ui.Log.Add($"{parsedIp} unblocked");
            }
            else
            {
                Ui.Log.Add(Resources._IncorrectValues);
            }
        }
    }
}