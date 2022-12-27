using System;

namespace MothManager.Core.Logger
{
    public class LogEntry
    {
        internal ConsoleColor? ForegroundColor { get; set; }
        internal ConsoleColor? BackgroundColor { get; set; }
        
        internal LogEntryType LogType { get; set; }
        internal string Source { get; set; }
        internal string Message { get; set; }

        internal LogEntry()
        {
            LogType = LogEntryType.Info;
            Message = "";
        }

        internal LogEntry(LogEntryType logType, string message)
        {
            LogType = logType;
            Message = message;
        }

        internal LogEntry(LogEntryType logType, string source, string message)
        {
            LogType = logType;
            Source = source;
            Message = message;
        }

    }
}