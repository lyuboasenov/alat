using Alat.Logging.LogEntryFormatters;
using System;

namespace Alat.Logging.Appenders {
   public class DebugAppender : Appender {
      public LogEntryFormatter LogEntryFormatter { get; }

      public DebugAppender(LogEntryFormatter logEntryFormatter) {
         LogEntryFormatter = logEntryFormatter ?? throw new ArgumentNullException(nameof(logEntryFormatter));
      }

      public void Write(LogEntry entry) {
         System.Diagnostics.Debug.Write(LogEntryFormatter.Format(entry));
      }
   }
}
