using System;
using System.Collections.Generic;

namespace Alat.Logging {
   public class LogEntryData {
      public string Message { get; set; }
      public IEnumerable<LogEntryProperty> Properties { get; set; }

      public LogEntryData(string message) : this(message, Array.Empty<LogEntryProperty>()) { }
      public LogEntryData(string message, IEnumerable<LogEntryProperty> properties) {
         if (string.IsNullOrEmpty(message)) {
            throw new ArgumentNullException(nameof(message));
         }

         Message = message;
         Properties = properties;
      }
   }
}