using Lanchat.Console.Ui;

namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void PrivateMessage(string nickname, string message)
        {
            var node = program.Network.Methods.GetNode(nickname);
            if (node != null)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    Prompt.Alert("Message cannot be blank");
                }
                else
                {
                    Prompt.Out(message, null, program.Config.Nickname + " -> " + nickname);
                    node.SendPrivate(message);
                }
            }
            else
            {
                Prompt.Alert("User not found");
            }
        }
    }
}