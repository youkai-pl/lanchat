using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Nick
    {
        public static void Execute(string[] args, Config config)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Connect);
                return;
            }

            if (args.Length < 1)
            {
                Ui.Log.Add(Resources.Manual_Nick);
                return;
            }

            var nickname = args[0].Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {
                Ui.Log.Add(Resources._WrongNickname);
            }
            else
            {
                config.Nickname = nickname;
                Ui.Log.Add(Resources._SelfNicknameChanged);
                Ui.PromptIndicator.Text = $"[{config.Nickname}]> ";
            }
        }
    }
}