using System;
using System.Collections.Generic;

namespace Alat.Logging {
   public class LogEntryProperty {
      public string Name { get; }
      public string Value { get; }
      public IEnumerable<LogEntryProperty> Children { get; }

      public LogEntryProperty(string name, string value) {
         if (string.IsNullOrEmpty(name)) {
            throw new ArgumentNullException(nameof(name));
         }
         if (string.IsNullOrEmpty(value)) {
            throw new ArgumentNullException(nameof(value));
         }

         Name = name;
         Value = value;
      }

      public LogEntryProperty(string name, IEnumerable<LogEntryProperty> children) {
         if (string.IsNullOrEmpty(name)) {
            throw new ArgumentNullException(nameof(name));
         }

         Name = name;
         Children = children ?? throw new ArgumentNullException(nameof(children));
      }
   }
}