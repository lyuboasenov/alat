using Alat.Logging.Appenders;
using Alat.Logging.DataConverters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Logging {
   public sealed class Settings {
      public IEnumerable<IAppender> Appenders { get; }
      public IEnumerable<KeyValuePair<Type, IDataConverter>> DataConverters { get; }
      public Level Level { get; }
      public bool IncludeStackTrace { get; }

      private Settings(IEnumerable<IAppender> appenders,
         IEnumerable<KeyValuePair<Type, IDataConverter>> dataConverters,
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
         IAppender appender,
         bool includeStackTrace = false) {
         return FromAppenders(level, 
            new IAppender[] { appender },
            includeStackTrace);
      }

      public static Settings FromAppender(Level level,
         IAppender appender,
         IEnumerable<KeyValuePair<Type, IDataConverter>> toLogEntryDataConverters,
         bool includeStackTrace = false) {
         return FromAppenders(level, 
            new IAppender[] { appender },
            toLogEntryDataConverters,
            includeStackTrace);
      }

      public static Settings FromAppenders(Level level,
         IEnumerable<IAppender> loggerAppenders,
         bool includeStackTrace = false) {
         return new Settings(loggerAppenders,
            Array.Empty<KeyValuePair<Type, IDataConverter>>(),
            level,
            includeStackTrace);
      }

      public static Settings FromAppenders(Level level,
         IEnumerable<IAppender> loggerAppenders,
         IEnumerable<KeyValuePair<Type, IDataConverter>> toLogEntryDataConverters,
         bool includeStackTrace = false) {
         return new Settings(loggerAppenders,
            toLogEntryDataConverters,
            level,
            includeStackTrace);
      }
   }
}
