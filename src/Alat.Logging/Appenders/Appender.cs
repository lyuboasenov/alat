using Alat.Logging.LogEntryFormatters;

namespace Alat.Logging.Appenders {
   public interface Appender {
      LogEntryFormatter LogEntryFormatter { get; }
      void Write(LogEntry entry);
   }
}