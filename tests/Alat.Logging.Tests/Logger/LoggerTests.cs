using System.Linq;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public class LoggerTests : BaseLoggerTest {

      protected override void MessageAssertion(Level level) {
         var logEntry = Appender.LoggedEntries.Last();

         Assert.Equal(Message, logEntry.Data.Message);
         Assert.Equal(level, logEntry.Level);
         Assert.True(string.IsNullOrEmpty(logEntry.StackTrace));
      }

      protected override void ObjectAssertion(Level level) {
         var logEntry = Appender.LoggedEntries.Last();

         Assert.Equal(MessageObj.ToString(), logEntry.Data.Message);
         Assert.Equal(level, logEntry.Level);
         Assert.True(string.IsNullOrEmpty(logEntry.StackTrace));
      }
   }
}
