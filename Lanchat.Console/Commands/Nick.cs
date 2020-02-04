using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Nick(string nick)
        {
            if (!string.IsNullOrEmpty(nick) && nick.Length < 20)
            {
                Config.Nickname = nick;
                program.Network.Nickname = nick;
                Prompt.Notice("Nickname changed");
            }
            else
            {
                Prompt.Alert("Nick cannot be blank or longer than 20 characters");
            }
        }
    }
}