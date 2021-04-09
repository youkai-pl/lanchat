using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;

namespace Lanchat.ClientCore
{
    public static class Logger
    {
        public static void StartLogging()
        {
            AppDomain.CurrentDomain.FirstChanceException += OnFirstChanceException;
            var logPath = $"{ConfigManager.DataPath}/{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log";
            Trace.Listeners.Add(new FileTraceListener(logPath));
            Trace.IndentSize = 11;
            Trace.AutoFlush = true;
            Trace.WriteLine("Logging started");
        }

        public static void StopLogging()
        {
            AppDomain.CurrentDomain.FirstChanceException -= OnFirstChanceException;
        }

        private static void OnFirstChanceException(object sender, FirstChanceExceptionEventArgs e)
        {
            if (e.Exception.Source != null && e.Exception.Source.StartsWith("Lanchat"))
                Trace.WriteLine(e.Exception);
        }

        public static void DeleteOldLogs(int maxLogsCount)
        {
            foreach (var fi in new DirectoryInfo(ConfigManager.DataPath)
                .GetFiles("*.log")
                .OrderByDescending(x => x.LastWriteTime)
                .Skip(maxLogsCount))
                fi.Delete();
        }
    }
}