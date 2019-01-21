using Alat.Logging.LogEntryFormatters;
using System;

namespace Alat.Logging.Appenders {
   public class DebugAppender : IAppender {
      public ILogEntryFormatter LogEntryFormatter { get; }

      public DebugAppender(ILogEntryFormatter logEntryFormatter) {
         LogEntryFormatter = logEntryFormatter ?? throw new ArgumentNullException(nameof(logEntryFormatter));
      }

      public void Write(LogEntry entry) {
         System.Diagnostics.Debug.Write(LogEntryFormatter.Format(entry));
      }
   }
}
