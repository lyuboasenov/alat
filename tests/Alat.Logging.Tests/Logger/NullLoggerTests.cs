using System;
using Alat.Logging.Appenders;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public class NullLoggerTests : BaseLoggerTest {
      protected override string Message { get; }
      protected override object MessageObj { get; }

      public NullLoggerTests() {
         Message = null;
         MessageObj = null;
      }

      [Fact]
      public override void LogLevel() {
         Assert.Throws<ArgumentNullException>(() => { base.LogLevel(); });
      }

      [Fact]
      public override void LogLevelObject() {
         Assert.Throws<ArgumentNullException>(() => { base.LogLevelObject(); });
      }

      [Fact]
      public void LogNullLevel() {
         Assert.Throws<ArgumentNullException>(() => { Logger.Log("dummy", null); });
      }

      [Fact]
      public void LogNullLevelObject() {
         Assert.Throws<ArgumentNullException>(() => { Logger.Log(new object(), null); });
      }

      protected override void LogMessageCall(Action<string> logFunc, Level level) {
         Assert.Throws<ArgumentNullException>(() => { base.LogMessageCall(logFunc, level); });
      }

      protected override void LogObjectCall(Action<object> logFunc, Level level) {
         Assert.Throws<ArgumentNullException>(() => { base.LogObjectCall(logFunc, level); });
      }

      protected override void MessageAssertion(Level level) {
         Assert.Empty(Appender.LoggedEntries);
      }

      protected override void ObjectAssertion(Level level) {
         Assert.Empty(Appender.LoggedEntries);
      }

      protected override Logging.Settings GetLoggerSettings(Appender appender) {
         return Logging.Settings.FromAppender(Level.All, appender);
      }
   }
}
