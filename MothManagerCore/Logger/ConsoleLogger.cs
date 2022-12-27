using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MothManager.Core.Logger
{
    public class ConsoleLogger : ILogger
    {
        public class LogEntryFormatter
        {
            public string FormatString { get; set; }
            public ConsoleColor? BackgroundColor { get; set; }
            public ConsoleColor? ForegroundColor { get; set; }

            public LogEntryFormatter(string formatString = "[{0}] {1}{2}", ConsoleColor? backgroundColor = null,
                ConsoleColor? foregroundColor = null)
            {
                FormatString = formatString;
                BackgroundColor = backgroundColor;
                ForegroundColor = foregroundColor;
            }

            public void WriteToConsole(LogEntry entry)
            {
                Console.BackgroundColor = entry.BackgroundColor ?? BackgroundColor ?? Console.BackgroundColor;
                Console.ForegroundColor = entry.ForegroundColor ?? ForegroundColor ?? Console.ForegroundColor;
                
                var formattedString = string.Format(FormatString, DateTime.Now.ToShortTimeString(), string.IsNullOrWhiteSpace(entry.Source) ? "" : $"({entry.Source}) - ", entry.Message);

                WriteToConsole(formattedString);
                
                Console.WriteLine();

                Console.ResetColor();
            }

            private void WriteToConsole(string entryText)
            {
                while (entryText.Length > 0)
                {

                    var openTagRegex = new Regex(@"(?s)<(?<tag>[^\\<]*?):(?<paran>.*?)>");
                    var openMatch = openTagRegex.Match(entryText);

                    if (openMatch.Success)
                    {
                        Console.Write(entryText.Substring(0, openMatch.Index));
                        
                        var tag = openMatch.Groups["tag"];
                        var param = openMatch.Groups["paran"];

                        var nextTagRegex = new Regex($"(?s)(<{tag})|(<\\/{tag}>)");
                        var subStringStart = openMatch.Index + openMatch.Length;
                        var closeMatch = nextTagRegex.Match(entryText, openMatch.Index + openMatch.Length);

                        var depth = 1;

                        while (closeMatch.Success)
                        {
                            if (closeMatch.Groups[1].Length > 0)
                            {
                                depth++;
                            }
                            else if (closeMatch.Groups[2].Length > 0)
                            {
                                depth--;
                            }

                            if (depth == 0)
                            {
                                break;
                            }

                            closeMatch = closeMatch.NextMatch();
                        }

                        if (depth == 0)
                        {
                            using (GetTagScope(tag.Value, param.Value))
                            {
                                WriteToConsole(entryText.Substring(subStringStart, closeMatch.Index - subStringStart));
                            }

                            entryText = entryText.Substring(closeMatch.Index + closeMatch.Length);
                        }
                        else
                        {
                            Console.Write(entryText);
                            entryText = "";
                        }
                    }
                    else
                    {
                        Console.Write(entryText);
                        entryText = "";
                    }
                }
            }

            private TagScope GetTagScope(string tag, string param)
            {
                tag = tag.Trim().ToLower();

                switch (tag)
                {
                    case "color":
                        return new ConsoleColorTagScope(param);
                    
                    default:
                        return new TagScope();
                }
                
            }
        }

        private readonly Dictionary<LogEntryType, LogEntryFormatter> entryFormatters =
            new Dictionary<LogEntryType, LogEntryFormatter>()
            {
                { LogEntryType.Info, new LogEntryFormatter() },
                {
                    LogEntryType.Warning,
                    new LogEntryFormatter("[{0}] *WARNING* {1}{2}", ConsoleColor.Black, ConsoleColor.Yellow)
                },
                {
                    LogEntryType.Error,
                    new LogEntryFormatter("[{0}] **ERROR** {1}{2}", ConsoleColor.DarkRed, ConsoleColor.White)
                },
                {
                    LogEntryType.SimpleError,
                    new LogEntryFormatter("[{0}] **ERROR** {1}{2}", ConsoleColor.Black, ConsoleColor.Red)
                },
                {
                    LogEntryType.Raw,
                    new LogEntryFormatter("{2}")
                }
            };

        private readonly BlockingCollection<LogEntry> pendingLogEntries;
        private object _lock = new object();

        // Constructor create the thread that wait for work on .GetConsumingEnumerable()
        public ConsoleLogger()
        {
            this.pendingLogEntries = new BlockingCollection<LogEntry>();
        }

        public void Initialize()
        {
            Processing = true;
            Task.Run(Process);
        }

        public void Cleanup()
        {
            pendingLogEntries.CompleteAdding();
            Processing = false;
        }

        private async void Process()
        {
            var tempFormatter = new LogEntryFormatter();
            tempFormatter.BackgroundColor = ConsoleColor.DarkBlue;
            tempFormatter.ForegroundColor = ConsoleColor.Cyan;
            
            while (Processing || pendingLogEntries.Count > 0)
            {
                var entry = pendingLogEntries.Take();

                if (!entryFormatters.TryGetValue(entry.LogType, out var formatter))
                {
                    formatter = new LogEntryFormatter();
                    entryFormatters.Add(entry.LogType, formatter);
                }

                formatter.WriteToConsole(entry);
            }
        }

        public bool Processing { get; set; }

        // ~ConsoleLogger()
        // {
        //     // Free the writing thread
        //     pendingLogEntries.();
        // }

        public void WriteLine(string msg)
        {
            pendingLogEntries.Add(new LogEntry(LogEntryType.Info, msg));
        }

        public void WriteLine(LogEntryType type, string msg)
        {
            pendingLogEntries.Add(new LogEntry(type, msg));
        }

        public void WriteLine(string source, string msg)
        {
            pendingLogEntries.Add(new LogEntry(LogEntryType.Info, source, msg));
        }

        public void WriteLine(LogEntryType type, string source, string msg)
        {
            pendingLogEntries.Add(new LogEntry(type, source, msg));
        }

        public void Dispose()
        {
            Cleanup();
        }
    }

    internal class ConsoleColorTagScope : TagScope
    {
        private readonly ConsoleColor _previousForeground;
        private readonly ConsoleColor _previousBackground;
        
        public ConsoleColorTagScope(string param)
        {
            _previousForeground = Console.ForegroundColor;
            _previousBackground = Console.BackgroundColor;

            var tokens = param.Split(',', StringSplitOptions.TrimEntries);

            if (tokens.Length >= 1)
            {
                if (Enum.TryParse<ConsoleColor>(tokens[0], true, out var fg))
                {
                    Console.ForegroundColor = fg;
                }
                
                if (tokens.Length >= 2)
                {
                    if (Enum.TryParse<ConsoleColor>(tokens[1], true, out var bg))
                    {
                        Console.BackgroundColor = bg;
                    }
                }
            }
        }

        public override void Dispose()
        {
            Console.ForegroundColor = _previousForeground;
            Console.BackgroundColor = _previousBackground;
            
            base.Dispose();
        }
    }
}