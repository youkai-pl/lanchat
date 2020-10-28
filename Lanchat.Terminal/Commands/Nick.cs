using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Nick
    {
        public static void Execute(string[] args)
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
                Ui.Log.Add(Resources.Info_WrongNickname);
            }
            else
            {
                Program.Config.Nickname = nickname;
                Ui.Log.Add(Resources.Info_SelfNicknameChanged);
                Ui.PromptIndicator.Text = $"[{Program.Config.Nickname}] ";
            }
        }
    }
}