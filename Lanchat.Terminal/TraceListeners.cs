using System;
using System.Diagnostics;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal
{
    public class TerminalTraceListener : TextWriterTraceListener
    {
        public override void WriteLine(string message)
        {
            Ui.Log.Add(message);
        }
    }
}