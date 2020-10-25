using Lanchat.Terminal.Properties;
using Lanchat.Terminal.Ui;

namespace Lanchat.Terminal.Commands
{
    public static class Nick
    {
        public static void Execute(string[] args, Config config)
        {
            if (args == null || args.Length < 1)
            {
                Prompt.Log.Add(Resources.Manual_Connect);
                return;
            }

            if (args.Length < 1)
            {
                Prompt.Log.Add(Resources.Manual_Nick);
                return;
            }

            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Prompt.Log.Add(Resources._WrongNickname);
            }
            else
            {
                config.Nickname = nickname;
                Prompt.Log.Add(Resources._SelfNicknameChanged);
                Prompt.PromptIndicator.Text = $"[{config.Nickname}]> ";
            }
        }
    }
}