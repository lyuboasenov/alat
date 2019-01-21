using Alat.Logging.LogEntryFormatters;

namespace Alat.Logging.Appenders {
   public interface IAppender {
      ILogEntryFormatter LogEntryFormatter { get; }
      void Write(LogEntry entry);
   }
}