using System;

namespace Alat.Logging {
   public class LogEntry {
      public DateTime Timestamp { get; set; }
      public Level Level { get; set; }
      public LogEntryData Data { get; }
      public string StackTrace { get; }

      public LogEntry(DateTime timestamp, Level level, LogEntryData data, string stackTrace = "") {
         Timestamp = timestamp;

         Level = level ?? throw new ArgumentNullException(nameof(level));
         Data = data ?? throw new ArgumentNullException(nameof(data));
         StackTrace = stackTrace;
      }
   }
}
