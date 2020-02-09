using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Nick(string nick)
        {
            var nickname = nick.Trim();
            if (nickname.Length >= 20 || string.IsNullOrWhiteSpace(nickname))
            {     
                Prompt.Alert("Nick cannot be blank or longer than 20 characters");
            }
            else
            {
                Config.Nickname = nickname;
                program.Network.Nickname = nickname;
                Prompt.Notice("Nickname changed");
            }
        }
    }
}