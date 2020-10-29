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

    public class FileTraceListener : TextWriterTraceListener
    {
        public FileTraceListener(string fileName) : base(fileName)
        { }

        public override void WriteLine(string message)
        {
            if (IndentLevel > 0)
            {
                base.WriteLine(message);
            }
            else
            {
                base.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ") + message);
            }
        }
    }
}