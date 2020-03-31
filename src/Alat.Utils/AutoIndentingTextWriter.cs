using System.IO;

namespace Alat.Utils {
   public class AutoIndentingTextWriter : IndentingTextWriter {

      public char IncreaseIndentChar { get; set; } = '{';
      public char DecreaseIndentChar { get; set; } = '}';

      public AutoIndentingTextWriter(TextWriter writer) : base(writer) { }

      public override void Write(char value) {
         if (value == DecreaseIndentChar) {
            IndentLevel--;
         }

         base.Write(value);

         if (value == IncreaseIndentChar) {
            IndentLevel++;
         }
      }

      public override void Write(string s) {
         foreach (var ch in s) {
            Write(ch);
         }
      }
   }
}
