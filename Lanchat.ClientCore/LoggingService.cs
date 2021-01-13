using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Lanchat.ClientCore
{
    public static class LoggingService
    {
        public static void StartLogging()
        {
            AppDomain.CurrentDomain.FirstChanceException += (_, eventArgs) =>
            {
                Trace.WriteLine(eventArgs.Exception.ToString());
            };
            
            Trace.Listeners.Add(new FileTraceListener($"{Config.DataPath}/{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("Logging started");
        }
        
        public static void CleanLogs()
        {
            foreach (var fi in new DirectoryInfo(Config.DataPath)
                .GetFiles("*.log")
                .OrderByDescending(x => x.LastWriteTime)
                .Skip(5))
                fi.Delete();
        }
    }
}