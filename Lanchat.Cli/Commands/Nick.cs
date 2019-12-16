using Lanchat.Cli.ProgramLib;
using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public void Nick(string nick)
        {
            if (!string.IsNullOrEmpty(nick) && nick.Length < 20)
            {
                Config.Nickname = nick;
                program.Network.Nickname = nick;
                program.Network.Out.ChangeNickname(nick);
                Prompt.Notice("Nickname changed");
            }
            else
            {
                Prompt.Alert("Nick cannot be blank or longer than 20 characters");
            }
        }
    }
}