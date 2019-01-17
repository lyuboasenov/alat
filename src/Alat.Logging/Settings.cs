using Alat.Logging.Appenders;
using Alat.Logging.DataConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Logging {
   public sealed class Settings {
      public IEnumerable<Appender> Appenders { get; }
      public IEnumerable<KeyValuePair<Type, DataConverter>> DataConverters { get; }
      public Level Level { get; }
      public bool IncludeStackTrace { get; }

      private Settings(IEnumerable<Appender> appenders,
         IEnumerable<KeyValuePair<Type, DataConverter>> dataConverters,
         Level level,
         bool includeStackTrace) {

         if (null == appenders) {
            throw new ArgumentNullException(nameof(appenders));
         }

         if (!appenders.Any()) {
            throw new ArgumentException($"At least one appender is needed in {nameof(appenders)}");
         }

         if (appenders.Any(a => a == null)) {
            throw new ArgumentNullException($"{nameof(appenders)} contains null value");
         }

         if (null == dataConverters) {
            throw new ArgumentNullException(nameof(dataConverters));
         }

         var multipleConverters = dataConverters.GroupBy(p => p.Key).Where(g => g.Count() > 1);
         if (multipleConverters.Any()) {
            throw new ArgumentException($"Multiple converters supplied for type {multipleConverters.First().Key.FullName}");
         }

         Appenders = appenders.ToArray();
         DataConverters = dataConverters.ToArray();

         Level = level ?? throw new ArgumentNullException(nameof(level));
         IncludeStackTrace = includeStackTrace;
      }

      public static Settings DisableLogging() {
         return FromAppender(Level.Off, new VoidAppender(), false);
      }

      public static Settings FromAppender(Level level,
         Appender appender,
         bool includeStackTrace = false) {
         return FromAppenders(level, 
            new Appender[] { appender },
            includeStackTrace);
      }

      public static Settings FromAppender(Level level,
         Appender appender,
         IEnumerable<KeyValuePair<Type, DataConverter>> toLogEntryDataConverters,
         bool includeStackTrace = false) {
         return FromAppenders(level, 
            new Appender[] { appender },
            toLogEntryDataConverters,
            includeStackTrace);
      }

      public static Settings FromAppenders(Level level,
         IEnumerable<Appender> loggerAppenders,
         bool includeStackTrace = false) {
         return new Settings(loggerAppenders,
            Array.Empty<KeyValuePair<Type, DataConverter>>(),
            level,
            includeStackTrace);
      }

      public static Settings FromAppenders(Level level,
         IEnumerable<Appender> loggerAppenders,
         IEnumerable<KeyValuePair<Type, DataConverter>> toLogEntryDataConverters,
         bool includeStackTrace = false) {
         return new Settings(loggerAppenders,
            toLogEntryDataConverters,
            level,
            includeStackTrace);
      }
   }
}
