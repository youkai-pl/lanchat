using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Lanchat.Terminal.Ui
{
    public class TerminalTraceListener : TextWriterTraceListener
    {
        public TerminalTraceListener()
        {
        }

        public override void WriteLine(string message)
        {
            if (IndentLevel > 0)
            {
                Prompt.Log.Add(message, Prompt.OutputType.Clear);
            }
            else
            {
                Prompt.Log.Add(DateTime.Now.ToString("[HH:mm:ss] ", CultureInfo.CurrentCulture) + message, Prompt.OutputType.Clear);
            }
        }
    }

    public class FileTraceListener : TextWriterTraceListener
    {
        public FileTraceListener(string fileName) : base(fileName)
        {
        }

        public FileTraceListener(TextWriter writer) : base(writer)
        {
        }

        public override void WriteLine(string message)
        {
            if (IndentLevel > 0)
            {
                base.WriteLine(message);
            }
            else
            {
                base.WriteLine(DateTime.Now.ToString("[HH:mm:ss] ", CultureInfo.CurrentCulture) + message);
            }
        }
    }
}
