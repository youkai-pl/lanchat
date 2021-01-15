using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public static class Dnd
    {
        public static void Execute()
        {
            CoreConfig.Status = Status.DoNotDisturb;
            Ui.Status.Text = "Do not disturb";
        }
    }
}