using Lanchat.Common.NetworkLib;
using Lanchat.Terminal.Ui;
using System;

namespace Lanchat.Terminal.Commands
{
    public static class Nick
    {
        public static void Execute(string[] args, Config config, Network network)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            if (args.Length < 1)
            {
                Prompt.Log.Add(Properties.Resources.Manual_Nick);
                return;
            }

            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Prompt.Log.Add(Properties.Resources._WrongNickname, Prompt.OutputType.System);
            }
            else
            {
                config.Nickname = nickname;
                network.Nickname = nickname;
                Prompt.Log.Add(Properties.Resources._SelfNicknameChanged, Prompt.OutputType.System);
                Prompt.PromptIndicator.Text = $"[{config.Nickname}]> ";
            }
        }
    }
}
