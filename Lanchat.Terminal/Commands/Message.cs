using Lanchat.Common.NetworkLib;
using Lanchat.Terminal.Ui;
using System;
using System.Linq;

namespace Lanchat.Terminal.Commands
{
    public static class Message
    {
        public static void Execute(string[] args, Network network)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (args.Length < 2)
            {
                Prompt.Log.Add(Properties.Resources.Manual_Message);
                return;
            }

            var message = string.Join(" ", args.Skip(1).ToArray());

            var node = network.Methods.GetNode(args[0]);
            if (node != null)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    Prompt.Log.Add(Properties.Resources.Manual_Message);
                }
                else
                {
                    Prompt.Log.Add(message, Prompt.OutputType.Message, $"-> {node.Nickname}");
                    node.SendPrivate(message);
                }
            }
            else
            {
                Prompt.Log.Add(Properties.Resources._UserNotFound);
            }
        }
    }
}
