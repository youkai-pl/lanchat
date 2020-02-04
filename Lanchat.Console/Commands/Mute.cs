using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Mute(string nickname)
        {
            var user = program.Network.NodeList.Find(x => x.Nickname.Equals(nickname));
            if (user != null)
            {
                user.Mute = true;
                if (Config.Muted.Exists(x => x.Equals(user.Ip)))
                {
                    Prompt.Out("User already muted");
                }
                else
                {
                    Config.AddMute(user.Ip);
                    Prompt.Notice($"{nickname} muted");
                }
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}