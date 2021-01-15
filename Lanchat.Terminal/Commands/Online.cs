using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Online
    {
        public static void Execute()
        {
            CoreConfig.Status = Status.Online;
            Ui.Status.Text = "Online";
        }
    }
}