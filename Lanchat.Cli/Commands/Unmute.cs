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
                user.Mute = false;
                Prompt.Out($"{nickname} unmuted");
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}