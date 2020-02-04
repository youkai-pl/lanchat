using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Unmute(string nickname)
        {
            var user = program.Network.NodeList.Find(x => x.Nickname.Equals(nickname));
            if (user != null)
            {
                var userOnList = Config.Muted.Find(x => x.Equals(user.Ip));
                if (userOnList != null)
                {
                    user.Mute = false;
                    Config.RemoveMute(userOnList);
                    Prompt.Notice($"{nickname} unmuted");
                }
                else
                {
                    Prompt.Notice("User is not muted");
                }
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}