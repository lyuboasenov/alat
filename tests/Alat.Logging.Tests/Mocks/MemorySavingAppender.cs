using System.Collections.Generic;
using Alat.Logging.LogEntryFormatters;

namespace Alat.Logging.Tests {
   public class MemorySavingAppender : Appenders.IAppender {
      public ILogEntryFormatter LogEntryFormatter { get; }
      public IList<LogEntry> LoggedEntries { get; } = new List<LogEntry>();

      public void Write(LogEntry entry) {
         LoggedEntries.Add(entry);
      }
   }
}
