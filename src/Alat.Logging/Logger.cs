using Alat.Logging.Appenders;
using Alat.Logging.DataConverters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Alat.Logging {
   public class Logger : ILogger {
      private Level DefaultLogLevel => Level.Trace;

      private IEnumerable<IAppender> Appenders { get; }
      private IDictionary<Type, IDataConverter> DataConverters { get; }
      private Level Level { get; }
      private bool IncludeStackTrace { get; }

      public Logger(Settings settings) {
         if (null == settings) {
            throw new ArgumentNullException(nameof(settings));
         }

         if(null == settings.Appenders) {
            throw new ArgumentNullException(nameof(settings.Appenders));
         }

         if (!settings.Appenders.Any()) {
            throw new ArgumentException(
               $"At least one appender should be available in {nameof(settings.Appenders)}");
         }

         Appenders = settings.Appenders.ToArray();
         DataConverters = new Dictionary<Type, IDataConverter>();
         if (null != settings.DataConverters) {
            foreach (var pair in settings.DataConverters) {
               if (DataConverters.ContainsKey(pair.Key)) {
                  throw new ArgumentException($"Multiple converters given for {pair.Key} type");
               }

               DataConverters.Add(pair.Key, pair.Value);
            }
         }

         Level = settings.Level;
         IncludeStackTrace = settings.IncludeStackTrace;
      }

      public void Debug(string message) {
         LogObject(message, Level.Debug);
      }

      public void Debug(object obj) {
         LogObject(obj, Level.Debug);
      }

      public void Error(string message) {
         LogObject(message, Level.Error);
      }

      public void Error(object obj) {
         LogObject(obj, Level.Error);
      }

      public void Fatal(string message) {
         LogObject(message, Level.Fatal);
      }

      public void Fatal(object obj) {
         LogObject(obj, Level.Fatal);
      }

      public void Info(string message) {
         LogObject(message, Level.Info);
      }

      public void Info(object obj) {
         LogObject(obj, Level.Info);
      }

      public void Log(object obj) {
         LogObject(obj, DefaultLogLevel);
      }

      public void Log(object obj, Level level) {
         LogObject(obj, level);
      }

      public void Log(string message) {
         LogObject(message, DefaultLogLevel);
      }

      public void Log(string message, Level level) {
         LogObject(message, level);
      }

      public void Warn(string message) {
         LogObject(message, Level.Warn);
      }

      public void Warn(object obj) {
         LogObject(obj, Level.Warn);
      }

      private void LogObject(object obj, Level level) {
         if (null == level) {
            throw new ArgumentNullException(nameof(level));
         }

         if (level > Level) {
            if (null == obj) {
               throw new ArgumentNullException(nameof(obj));
            }

            if (obj is string && string.IsNullOrEmpty((string)obj)) {
               throw new ArgumentNullException(nameof(obj));
            }

            var logEntry = CreateLogEntry(obj, level);

            foreach (var appender in Appenders) {
               appender.Write(logEntry);
            }
         }
      }


      private LogEntry CreateLogEntry(object obj, Level level) {
         var type = obj.GetType();

         LogEntryData data = null;
         if (DataConverters.ContainsKey(type)) {
            data = DataConverters[type].Convert(obj);
         } else {
            data = new LogEntryData(obj.ToString());
         }

         string stackTrace = string.Empty;
         if (IncludeStackTrace) {
            stackTrace = GetStackTrace();
         }

         return new LogEntry(DateTime.Now, level, data, stackTrace);
      }

      private static string GetStackTrace() {
         StringBuilder formattedStackTrace = new StringBuilder();
         var stackTrace = new StackTrace(4, true);

         return stackTrace.ToString();
      }
   }
}
