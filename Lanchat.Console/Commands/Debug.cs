namespace Lanchat.Console.Commands
{
    public partial class Command
    {
        public void Debug()
        {
            if (program.DebugMode)
            {
                program.DebugMode = false;
            }
            else
            {
                program.DebugMode = true;
            }
        }
    }
}