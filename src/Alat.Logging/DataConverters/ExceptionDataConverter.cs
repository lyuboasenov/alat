using System;
using System.Collections.Generic;

namespace Alat.Logging.DataConverters {
   public class ExceptionDataConverter : DataConverter {
      public LogEntryData Convert(object obj) {
         if (null == obj) {
            throw new ArgumentNullException(nameof(obj));
         }

         var exception = obj as Exception;
         if (null == exception) {
            throw new ArgumentException($"{nameof(obj)} is not Exception");
         }

         return new LogEntryData(exception.Message, CollectProperties(exception));
      }

      private IEnumerable<LogEntryProperty> CollectProperties(Exception exception) {
         var properties = new List<LogEntryProperty> {
            new LogEntryProperty(nameof(exception.Message), exception.Message),
            new LogEntryProperty("Type", exception.GetType().FullName),
         };

         if (!string.IsNullOrEmpty(exception.Source)) {
            properties.Add(new LogEntryProperty(nameof(exception.Source), exception.Source));
         }
         if (!string.IsNullOrEmpty(exception.StackTrace)) {
            properties.Add(new LogEntryProperty(nameof(exception.Source), exception.StackTrace));
         }
         if (null != exception.InnerException) {
            properties.Add(
               new LogEntryProperty(nameof(exception.InnerException), CollectProperties(exception.InnerException)));
         }

         return properties;
      }
   }
}
