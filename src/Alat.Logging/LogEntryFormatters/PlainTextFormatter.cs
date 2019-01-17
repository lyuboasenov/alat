using System.Collections.Generic;
using System.Text;

namespace Alat.Logging.LogEntryFormatters {
   public class PlainTextFormatter : LogEntryFormatter {

      private StringBuilder FormatedLogEntry { get; }

      public PlainTextFormatter() {
         FormatedLogEntry = new StringBuilder();
      }

      public string Format(LogEntry logEntry) {
         FormatedLogEntry.Clear();
         try {
            FormatedLogEntry.AppendLine("-----------------------------------------------------");
            FormatedLogEntry.AppendLine($"   {logEntry.Timestamp:G}");
            FormatedLogEntry.AppendLine($"   {logEntry.Level}");
            FormatedLogEntry.AppendLine($"      {logEntry.Data.Message}");
            if (null != logEntry.Data.Properties) {
               FormatedLogEntry.Append("         ");
               FormatProperties(logEntry.Data.Properties);
            }
            if (!string.IsNullOrEmpty(logEntry.StackTrace)) {
               FormatedLogEntry.AppendLine($"      {logEntry.StackTrace}");
            }
            FormatedLogEntry.AppendLine("-----------------------------------------------------");

            return FormatedLogEntry.ToString();
         } finally {
            FormatedLogEntry.Clear();
         }
      }

      private void FormatProperties(IEnumerable<LogEntryProperty> properties, int tabs = 3) {
         var skipLeadingSpaces = true;
         foreach(var property in properties) {
            if(!skipLeadingSpaces) {
               for (int i = 0; i < tabs; i++) FormatedLogEntry.Append("   ");
            }
            skipLeadingSpaces = false;
            if (property.Children == null) {
               FormatedLogEntry.AppendLine($"{property.Name}: {property.Value}");
            } else {
               FormatedLogEntry.Append($"{property.Name}:");
               FormatProperties(property.Children, tabs + 1);
            }
         }
      }
   }
}
