using Lanchat.Cli.PromptLib;

namespace Lanchat.Cli.Commands
{
    public partial class Command
    {
        public void Mute(string nickname)
        {
            var user = program.Network.NodeList.Find(x => x.Nickname.Equals(nickname));
            if (user != null)
            {
                user.Mute = true;
                Prompt.Out($"{nickname} muted");
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}
