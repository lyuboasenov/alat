using Alat.Logging.Appenders;
using System.Linq;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public class LoggerStackTraceTests : BaseLoggerTest {

      protected override void MessageAssertion(Level level) {
         var logEntry = Appender.LoggedEntries.Last();

         Assert.Equal(Message, logEntry.Data.Message);
         Assert.Equal(level, logEntry.Level);
         Assert.False(string.IsNullOrEmpty(logEntry.StackTrace));
      }

      protected override void ObjectAssertion(Level level) {
         var logEntry = Appender.LoggedEntries.Last();

         Assert.Equal(MessageObj.ToString(), logEntry.Data.Message);
         Assert.Equal(level, logEntry.Level);
         Assert.False(string.IsNullOrEmpty(logEntry.StackTrace));
      }

      protected override Logging.Settings GetLoggerSettings(IAppender appender) {
         return Logging.Settings.FromAppender(Level.All, appender, true);
      }
   }
}
