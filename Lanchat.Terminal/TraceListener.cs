using System.Diagnostics;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public class TraceListener : TextWriterTraceListener
    {
        public override void WriteLine(string message)
        {
            TabsManager.DebugView.AddToLog(message);
        }
    }
}