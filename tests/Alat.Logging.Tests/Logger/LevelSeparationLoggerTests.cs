using System;
using System.Linq;
using Alat.Logging.Appenders;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public class LevelSeparationLoggerTests : BaseLoggerTest {

      private Level Level { get; } = Level.Info;

      protected override void LogMessageCall(Action<string> logFunc, Level level) {
         Appender.LoggedEntries.Clear();
         base.LogMessageCall(logFunc, level);
      }

      protected override void LogObjectCall(Action<object> logFunc, Level level) {
         Appender.LoggedEntries.Clear();
         base.LogObjectCall(logFunc, level);
      }

      protected override void MessageAssertion(Level level) {
         if (Level < level) {
            var logEntry = Appender.LoggedEntries.Last();

            Assert.Equal(Message, logEntry.Data.Message);
            Assert.Equal(level, logEntry.Level);
         } else {
            Assert.Empty(Appender.LoggedEntries);
         }
      }

      protected override void ObjectAssertion(Level level) {
         if (Level < level) {
            var logEntry = Appender.LoggedEntries.Last();

            Assert.Equal(MessageObj.ToString(), logEntry.Data.Message);
            Assert.Equal(level, logEntry.Level);
         } else {
            Assert.Empty(Appender.LoggedEntries);
         }
      }

      protected override Logging.Settings GetLoggerSettings(Appender appender) {
         return Logging.Settings.FromAppender(Level, appender);
      }
   }
}
