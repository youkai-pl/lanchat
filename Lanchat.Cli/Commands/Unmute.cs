using Lanchat.Cli.ProgramLib;
using Lanchat.Cli.Ui;

namespace Lanchat.Cli.Commands
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
                    Config.Muted.Remove(userOnList);
                    Config.Save();
                    Prompt.Out($"{nickname} unmuted");
                } else
                {
                    Prompt.Out("User is not muted");
                }
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}