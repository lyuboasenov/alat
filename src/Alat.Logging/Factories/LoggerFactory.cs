using Alat.Logging.DataConverters;
using System;
using System.Collections.Generic;

namespace Alat.Logging.Factories {
   public sealed class LoggerFactory {
      public static ILogger GetDebugLogger(Level level) {
         var logEntryFormatter = new LogEntryFormatters.PlainTextFormatter();
         var debugAppender = new Appenders.DebugAppender(logEntryFormatter);
         var dataConverters = new KeyValuePair<Type, IDataConverter>[] {
               new KeyValuePair<Type, IDataConverter>(typeof(string), new ExceptionDataConverter())
            };

         var settings = Settings.FromAppender(level, debugAppender, dataConverters, true);
         return new Logger(settings);
      }

      public static ILogger GetDisabledLogger() {
         var settings = Settings.DisableLogging();
         return new Logger(settings);
      }
   }
}
