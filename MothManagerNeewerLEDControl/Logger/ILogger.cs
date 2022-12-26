using System;

namespace MothManagerNeewerLEDControl.Logger
{
    public interface ILogger : IDisposable
    {
        void WriteLine(string msg);
        void WriteLine(LogEntryType type, string msg);
        void WriteLine(string source, string msg);
        void WriteLine(LogEntryType type, string source, string msg);
        void Initialize();
        void Cleanup();
    }

    public static class Logger
    {
        private static ILogger? ActiveLogger { get; set; }

        public static void SetLogger(ILogger newLogger)
        {
            if (ActiveLogger != null)
            {
                ActiveLogger.Cleanup();
            }

            ActiveLogger = newLogger;
            ActiveLogger.Initialize();
        }
        

        public static void WriteLine(string msg)
        {
            ActiveLogger?.WriteLine(msg);    
        }

        public static void WriteLine(LogEntryType type, string msg)
        {
            ActiveLogger?.WriteLine(type, msg);
        }

        public static void WriteLine(string source, string msg)
        {
            ActiveLogger?.WriteLine(source, msg);
        }

        public static void WriteLine(LogEntryType type, string source, string msg)
        {
            ActiveLogger?.WriteLine(type, source, msg);
        }
    }
}