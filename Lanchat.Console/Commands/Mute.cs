using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Mute(string nickname)
        {
            var node = program.Network.Methods.GetNode(nickname);
            if (node != null)
            {
                node.Mute = true;
                if (program.Config.Muted.Exists(x => x.Equals(node.Ip)))
                {
                    Prompt.Out("User already muted");
                }
                else
                {
                    program.Config.AddMute(node.Ip);
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