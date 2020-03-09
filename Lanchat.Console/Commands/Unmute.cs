using Lanchat.Console.Ui;
using System.Net;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Unmute(string nickname)
        {
            var node = program.Network.Methods.GetNode(nickname);
            if (node != null)
            {
                var savedNode = program.Config.Muted.Find(x => IPAddress.Parse(x).Equals(node.Ip));
                if (savedNode != null)
                {
                    node.Mute = false;
                    program.Config.RemoveMute(IPAddress.Parse(savedNode));
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