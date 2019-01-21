using Alat.Logging.Appenders;
using Alat.Logging.DataConverters;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Alat.Logging.Tests.LoggerSettings {
   public class LoggerSettingsAppender {

      private Level Level { get; }

      public LoggerSettingsAppender() {
         Level = Level.Info;
      }

      [Fact]
      public void DisableLogging() {
         var settings = Logging.Settings.DisableLogging();

         Assert.Equal(Level.Off, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is VoidAppender);
         Assert.False(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppender() {
         var settings = Logging.Settings.FromAppender(Level, new MemorySavingAppender());

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.False(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppenderIncludeStack() {
         var settings = Logging.Settings.FromAppender(Level, new MemorySavingAppender(), true);

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.True(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppenderConverter() {
         var settings = Logging.Settings.FromAppender(Level,
            new MemorySavingAppender(),
            new KeyValuePair<Type, IDataConverter>[] {
               new KeyValuePair<Type, IDataConverter>(typeof(string), new DataConverters.ExceptionDataConverter())
            });

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.False(settings.IncludeStackTrace);
         Assert.Single(settings.DataConverters);
      }

      [Fact]
      public void FromAppenderNullLevel() {
         Assert.Throws<ArgumentNullException>(() => 
            Logging.Settings.FromAppender(null, new MemorySavingAppender()));
      }

      [Fact]
      public void FromAppenderNullAppender() {
         Assert.Throws<ArgumentNullException>(() => Logging.Settings.FromAppender(Level, null));
      }

      [Fact]
      public void FromAppenderNullConverters() {
         Assert.Throws<ArgumentNullException>(() => 
            Logging.Settings.FromAppender(Level, new MemorySavingAppender(), null));
      }

      [Fact]
      public void FromAppenders() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender() });

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.False(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersIncludeStack() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender() },
            true);

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.True(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersToEntryConverter() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender() },
            Array.Empty<KeyValuePair<Type, IDataConverter>>());

         Assert.Equal(Level, settings.Level);
         Assert.Single(settings.Appenders, (appender) => appender is MemorySavingAppender);
         Assert.False(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersMultipleProviders() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender(), new VoidAppender() });

         Assert.Equal(Level, settings.Level);
         Assert.NotEmpty(settings.Appenders);
         Assert.Equal(2, settings.Appenders.Count());
         Assert.IsType<MemorySavingAppender>(settings.Appenders.First());
         Assert.IsType<VoidAppender>(settings.Appenders.Last());
         Assert.False(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersIncludeStackMultipleProviders() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender(), new VoidAppender() },
            true);

         Assert.Equal(Level, settings.Level);
         Assert.NotEmpty(settings.Appenders);
         Assert.Equal(2, settings.Appenders.Count());
         Assert.IsType<MemorySavingAppender>(settings.Appenders.First());
         Assert.IsType<VoidAppender>(settings.Appenders.Last());
         Assert.True(settings.IncludeStackTrace);
         Assert.Empty(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersToEntryConverterMultipleProviders() {
         var settings = Logging.Settings.FromAppenders(Level,
            new IAppender[] { new MemorySavingAppender(), new VoidAppender() },
            new KeyValuePair<Type, IDataConverter>[] {
               new KeyValuePair<Type, IDataConverter>(typeof(string), new DataConverters.ExceptionDataConverter())
            });

         Assert.Equal(Level, settings.Level);
         Assert.NotEmpty(settings.Appenders);
         Assert.Equal(2, settings.Appenders.Count());
         Assert.IsType<MemorySavingAppender>(settings.Appenders.First());
         Assert.IsType<VoidAppender>(settings.Appenders.Last());
         Assert.False(settings.IncludeStackTrace);
         Assert.Single(settings.DataConverters);
      }

      [Fact]
      public void FromAppendersNullLevel() {
         Assert.Throws<ArgumentNullException>(() => 
            Logging.Settings.FromAppenders(null, new IAppender[] { new MemorySavingAppender() }));
      }

      [Fact]
      public void FromAppendersNullAppender() {
         Assert.Throws<ArgumentNullException>(() => Logging.Settings.FromAppenders(Level, null));
      }

      [Fact]
      public void FromAppendersNullConverters() {
         Assert.Throws<ArgumentNullException>(() => 
            Logging.Settings.FromAppenders(Level, new IAppender[] { new MemorySavingAppender()}, null));
      }

      [Fact]
      public void FromAppendersDuplicateConverters() {
         Assert.Throws<ArgumentException>(() => Logging.Settings.FromAppenders(
            Level, 
            new IAppender[] { new MemorySavingAppender() },
            new List<KeyValuePair<Type, IDataConverter>> {
               new KeyValuePair<Type, IDataConverter>(typeof(string), new DataConverters.ExceptionDataConverter()),
               new KeyValuePair<Type, IDataConverter>(typeof(string), new DataConverters.ExceptionDataConverter())
            }));
      }

      [Fact]
      public void FromAppendersEmptyAppenders() {
         Assert.Throws<ArgumentException>(() => 
            Logging.Settings.FromAppenders(Level, new IAppender[] { }, null));
      }

      [Fact]
      public void FromAppendersConverterMultipleProvidersEdited() {
         var appenders = new List<IAppender>() {
            new MemorySavingAppender(), new VoidAppender()
         };

         var converters = new List<KeyValuePair<Type, IDataConverter>> {
            new KeyValuePair<Type, IDataConverter>(typeof(string), new DataConverters.ExceptionDataConverter())
         };

         var settings = Logging.Settings.FromAppenders(Level,
            appenders,
            converters);

         appenders.Clear();
         converters.Clear();

         Assert.Equal(Level, settings.Level);
         Assert.NotEmpty(settings.Appenders);
         Assert.Equal(2, settings.Appenders.Count());
         Assert.IsType<MemorySavingAppender>(settings.Appenders.First());
         Assert.IsType<VoidAppender>(settings.Appenders.Last());
         Assert.False(settings.IncludeStackTrace);
         Assert.Single(settings.DataConverters);
      }
   }
}
