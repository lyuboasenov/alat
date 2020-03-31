using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Alat.Utils {
   public class IndentingTextWriter : TextWriter {
      private readonly TextWriter _writer;
      private int _indentLevel;
      private string _indentString = "   ";
      private string _indent;
      private int _newLineIndex = -1;
      private bool _shouldIndent;

      public override Encoding Encoding { get { return _writer.Encoding; } }
      public override string NewLine { get { return _writer.NewLine; } }
      public int IndentLevel {
         get {
            return _indentLevel;
         }
         set {
            if (value < 0) {
               value = 0;
            }

            _indentLevel = value;
            SetIndent();
         }
      }
      public string IndentString {
         get {
            return _indentString;
         }
         set {
            _indentString = value ?? string.Empty;
            SetIndent();
         }
      }

      public IndentingTextWriter(TextWriter writer) : base(CultureInfo.InvariantCulture) {
         _writer = writer ?? throw new ArgumentNullException(nameof(writer));
      }

      public override void Write(string s) {
         if (_shouldIndent && s != NewLine) {
            WriteIndent();
         }

         _shouldIndent = s.EndsWith(NewLine);
         if (s == NewLine) {
            _writer.Write(s);
         } else {
            var lines = s.Split(new[] { NewLine }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length; i++) {
               _writer.Write(lines[i]);
               if (i < lines.Length - 1) {
                  _writer.Write(NewLine);
                  WriteIndent();
               }
            }
         }
      }

      public override void Write(char value) {
         if (_shouldIndent) {
            WriteIndent();
         }

         _writer.Write(value);
         _shouldIndent = ShouldIndent(value);
      }

      private void WriteIndent() {
         _writer.Write(_indent);
      }

      private bool ShouldIndent(char value) {
         var result = false;
         if (NewLine[_newLineIndex + 1] == value) {
            if (_newLineIndex + 1 < NewLine.Length - 1) {
               _newLineIndex++;
            } else {
               _newLineIndex = -1;
               result = true;
            }
         } else {
            _newLineIndex = -1;
         }
         return result;
      }

      public override void Close() {
         _writer.Close();
      }

      public override void Flush() {
         _writer.Flush();
      }
      private void SetIndent() {
         var sb = new StringBuilder();
         for (var i = 0; i < _indentLevel; i++) {
            sb.Append(_indentString);
         }
         _indent = sb.ToString();
      }
   }
}
