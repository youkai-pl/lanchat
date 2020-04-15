using Lanchat.Common.NetworkLib;
using Lanchat.Terminal.Ui;
using System;

namespace Lanchat.Terminal.Commands
{
    public static class Unmute
    {
        public static void Execute(string[] args, Config config, Network network)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (args.Length < 1)
            {
                Prompt.Log.Add(Properties.Resources.Manual_Mute);
                return;
            }

            var node = network.Methods.GetNode(args[0]);
            if (node != null)
            {
                node.Mute = false;
                if (config.Muted.Exists(x => x.Equals(node.Ip)))
                {
                    Prompt.Log.Add(Properties.Resources._NotMuted);
                }
                else
                {
                    config.RemoveMute(node.Ip);
                    Prompt.Log.Add($"{args[0]} unmuted");
                }
            }
            else
            {
                Prompt.Log.Add(Properties.Resources._UserNotFound);
            }
        }
    }
}
