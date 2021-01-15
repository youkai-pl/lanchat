using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Afk
    {
        public static void Execute()
        {
            CoreConfig.Status = Status.AwayFromKeyboard;
            Ui.Status.Text = "Away from keyboard";
        }
    }
}