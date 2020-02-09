using Lanchat.Console.ProgramLib;
using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Unmute(string nickname)
        {
            var node = program.Network.NodeList.Find(x => x.Nickname.Equals(nickname));
            if (node != null)
            {
                var savedNode = Config.Muted.Find(x => x.Equals(node.Ip));
                if (savedNode != null)
                {
                    node.Mute = false;
                    Config.RemoveMute(savedNode);
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