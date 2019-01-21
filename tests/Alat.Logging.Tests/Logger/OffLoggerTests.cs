using Alat.Logging.Appenders;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public class OffLoggerTests : BaseLoggerTest {

      protected override void MessageAssertion(Level level) {
         Assert.Empty(Appender.LoggedEntries);
      }

      protected override void ObjectAssertion(Level level) {
         Assert.Empty(Appender.LoggedEntries);
      }

      protected override Logging.Settings GetLoggerSettings(IAppender appender) {
         return Logging.Settings.FromAppender(Level.Off, appender);
      }
   }
}
