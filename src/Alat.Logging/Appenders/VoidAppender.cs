using Alat.Logging.LogEntryFormatters;
using System;

namespace Alat.Logging.Appenders {
   public class VoidAppender : Appender {
      public LogEntryFormatter LogEntryFormatter { get; }

      public VoidAppender() {

      }

      public void Write(LogEntry entry) {

      }
   }
}
