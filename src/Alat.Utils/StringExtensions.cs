using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Alat.PSSp.Functionality.Utils {
   public static class StringExtensions {
      private static readonly string[] _lineSplitter = new[] { Environment.NewLine };
      private static readonly char[] _newLineCharArray = new[] { '\r', '\n' };

      public static string Indent(this string text, int spaces = 3) {
         string result;
         if (string.IsNullOrEmpty(text)) {
            result = text;
         } else {
            var indent = new string(' ', spaces);
            var lines = text.Split(_lineSplitter, StringSplitOptions.None);
            var sb = new StringBuilder();

            foreach (var line in lines ?? Enumerable.Empty<string>()) {
               sb.Append(indent).AppendLine(line);
            }

            result = sb.ToString();
         }

         return result;
      }

      public static string FormatIndented(this string format, params object[] parameters) {
         return format.FormatIndented(3, 77, parameters);
      }

      public static string FormatIndented(this string format, int spaces, int maxLineLength, params object[] parameters) {
         return format.Format(parameters.Select(o => {
            string str;
            if (o == null) {
               str = string.Empty;
            } else if (o is string) {
               str = (string) o;
            } else if (o is IEnumerable) {
               str = string.Join(Environment.NewLine, ((IEnumerable) o).OfType<object>().Select(obj => obj.ToString()));
            } else {
               str = o.ToString();
            }
            return BreakStringIntoLengthRestrictedLines(str, maxLineLength).Indent(spaces);
         }).ToArray());
      }

      public static string Format(this string format, params object[] parameters) {
         return string.Format(format, parameters);
      }

      public static string Format(this string format, int maxLineLength, params object[] parameters) {
         return string.Format(format,
            parameters.Select(o => BreakStringIntoLengthRestrictedLines(o.ToString(), maxLineLength)).ToArray());
      }

      public static string RemoveTraillingNewLine(this string str) {
         return str.TrimEnd(_newLineCharArray);
      }

      public static string ToCamelCase(this string str) {
         string result = str;

         if (result.Length == 1) {
            result = result.ToLowerInvariant();
         } else if (result.Length > 1) {
            result = Char.ToLowerInvariant(result[0]) + result.Substring(1);
         }

         return result;
      }

      internal static string BreakStringIntoLengthRestrictedLines(string str, int maxLineLength) {
         string result;

         if (str.Contains(Environment.NewLine)) {
            result = BreakMultiLineIntoLengthRestrictedLines(str, maxLineLength);

         } else {
            result = BreakLineIntoLengthRestrictedLines(str, maxLineLength);
         }

         return result;
      }

      internal static string BreakLineIntoLengthRestrictedLines(string line, int maxLineLength) {

         if (line.Contains(Environment.NewLine)) { throw new ArgumentException(nameof(line)); }

         string result = line;
         if (line.Length > maxLineLength) {
            var sb = new StringBuilder(line.Length + 10);

            for (int i = 0; i < line.Length / maxLineLength; i++) {
               sb.Append(line, i * maxLineLength, maxLineLength).AppendLine();
            }
            int mod = line.Length % maxLineLength;
            if (mod != 0) {
               sb.Append(line, line.Length / maxLineLength * maxLineLength, mod).AppendLine();
            }

            result = sb.ToString().RemoveTraillingNewLine();
         }
         return result;
      }

      internal static string BreakMultiLineIntoLengthRestrictedLines(string str, int maxLineLength) {

         if (!str.Contains(Environment.NewLine)) { throw new ArgumentException(nameof(str)); }

         var lines = str.Split(_lineSplitter, StringSplitOptions.None);

         var sb = new StringBuilder(str.Length + (lines.Length * 2));

         foreach(var line in lines) {
            if (line.Length <= maxLineLength) {
               sb.AppendLine(line);
            } else {
               for (int i = 0; i < line.Length / maxLineLength; i++) {
                  sb.Append(line, i * maxLineLength, maxLineLength).AppendLine();
               }
               int mod = line.Length % maxLineLength;
               if (mod != 0) {
                  sb.Append(line, line.Length / maxLineLength * maxLineLength, mod).AppendLine();
               }
            }

         }

         return sb.ToString().RemoveTraillingNewLine();
      }

      public static string NormalizeLineEndings(this string self) {
         string result = null;

         if (self != null) {
            result = Regex.Replace(self, @"\r\n|\n\r|\n|\r", Environment.NewLine);
         }

         return result;
      }
   }
}
