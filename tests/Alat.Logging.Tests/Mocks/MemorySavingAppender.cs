using System.Collections.Generic;
using Alat.Logging.LogEntryFormatters;

namespace Alat.Logging.Tests {
   public class MemorySavingAppender : Appenders.Appender {
      public LogEntryFormatter LogEntryFormatter { get; }
      public IList<LogEntry> LoggedEntries { get; } = new List<LogEntry>();

      public void Write(LogEntry entry) {
         LoggedEntries.Add(entry);
      }
   }
}
