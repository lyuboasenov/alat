using Alat.Logging.Appenders;
using System;
using Xunit;

namespace Alat.Logging.Tests.Logger {
   public abstract class BaseLoggerTest {
      protected virtual string Message { get; } = "Default log message";
      protected virtual object MessageObj { get; } = "Default log message object";
      protected MemorySavingAppender Appender { get; }
      protected Logging.ILogger Logger { get; }

      protected BaseLoggerTest() {
         Appender = GetAppenderMock();
         Logger = GetLogger();
      }

      [Fact]
      public void Debug() {
         LogMessageCall(Logger.Debug, Level.Debug);
      }

      [Fact]
      public void DebugObject() {
         LogObjectCall(Logger.Debug, Level.Debug);
      }

      [Fact]
      public void Info() {
         LogMessageCall(Logger.Info, Level.Info);
      }

      [Fact]
      public void InfoObject() {
         LogObjectCall(Logger.Info, Level.Info);
      }

      [Fact]
      public void Warn() {
         LogMessageCall(Logger.Warn, Level.Warn);
      }

      [Fact]
      public void WarnObject() {
         LogObjectCall(Logger.Warn, Level.Warn);
      }

      [Fact]
      public void Error() {
         LogMessageCall(Logger.Error, Level.Error);
      }

      [Fact]
      public void ErrorObject() {
         LogObjectCall(Logger.Error, Level.Error);
      }

      [Fact]
      public void Fatal() {
         LogMessageCall(Logger.Fatal, Level.Fatal);
      }

      [Fact]
      public void FatalObject() {
         LogObjectCall(Logger.Fatal, Level.Fatal);
      }

      [Fact]
      public void Log() {
         LogMessageCall(Logger.Log, Level.Trace);
      }

      [Fact]
      public void LogObject() {
         LogObjectCall(Logger.Log, Level.Trace);
      }

      [Fact]
      public virtual void LogLevel() {
         Logger.Log(Message, Level.Fatal);
         MessageAssertion(Level.Fatal);
      }

      [Fact]
      public virtual void LogLevelObject() {
         Logger.Log(MessageObj, Level.Fatal);
         ObjectAssertion(Level.Fatal);
      }

      protected virtual void LogMessageCall(Action<string> logFunc, Level level) {
         logFunc(Message);

         MessageAssertion(level);
      }

      protected virtual void LogObjectCall(Action<object> logFunc, Level level) {
         logFunc(MessageObj);

         ObjectAssertion(level);
      }

      protected abstract void MessageAssertion(Level level);
      protected abstract void ObjectAssertion(Level level);

      protected virtual ILogger GetLogger() {
         return GetLogger(GetLoggerSettings(Appender));
      }

      protected virtual ILogger GetLogger(Settings settings) {
         return new Logging.Logger(settings);
      }

      protected virtual Settings GetLoggerSettings(IAppender appender) {
         return Settings.FromAppender(Level.All, appender);
      }

      protected static MemorySavingAppender GetAppenderMock() {
         return new MemorySavingAppender();
      }
   }
}
