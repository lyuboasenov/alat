using Alat.Logging.LogEntryFormatters;
using System;

namespace Alat.Logging.Appenders {
   public class VoidAppender : IAppender {
      public ILogEntryFormatter LogEntryFormatter { get; }

      public VoidAppender() {

      }

      public void Write(LogEntry entry) {

      }
   }
}
