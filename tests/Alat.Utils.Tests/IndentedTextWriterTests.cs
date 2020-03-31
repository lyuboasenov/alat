using Alat.Utils;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace Alat.Utils.Tests {
   [TestFixture]
   public class IndentedTextWriterTests {
      [Test]
      public void WriteLines() {
         // ARRANGE
         var sb = new StringBuilder();
         var sw = new StringWriter(sb);
         var writer = new IndentingTextWriter(sw);
         var expected = @"Line1
   Line2
      Line3
Line4
";

         // ACT
         writer.WriteLine("Line1");
         writer.IndentLevel++;
         writer.WriteLine("Line2");
         writer.IndentLevel++;
         writer.WriteLine("Line3");
         writer.IndentLevel = 0;
         writer.WriteLine("Line4");

         //ASSERT
         Assert.AreEqual(expected, sb.ToString());
      }

      [Test]
      public void Write() {
         // ARRANGE
         var sb = new StringBuilder();
         var sw = new StringWriter(sb);
         var writer = new IndentingTextWriter(sw);
         var expected = @"Line1
Line2
      Line3
      Line4
   Line5
Line6";

         // ACT
         writer.Write(@"Line1
Line2");
         writer.IndentLevel = 2;
         writer.WriteLine(@"
Line3
Line4");
         writer.IndentLevel = 1;
         writer.WriteLine("Line5");
         writer.IndentLevel = 0;
         writer.Write("Line6");

         //ASSERT
         Assert.AreEqual(expected, sb.ToString());
      }

      [Test]
      public void WriteByChar() {
         // ARRANGE
         var sb = new StringBuilder();
         var sw = new StringWriter(sb);
         var writer = new IndentingTextWriter(sw);
         var expected = @"Line1
Line2
      Line3
      Line4
   Line5
Line6";

         // ACT
         foreach(char ch in @"Line1
Line2") {
            writer.Write(ch);
         }
         writer.IndentLevel = 2;
         foreach (char ch in @"
Line3
Line4
") {
            writer.Write(ch);
         }
         writer.IndentLevel = 1;
         foreach (char ch in @"Line5
") {
            writer.Write(ch);
         }
         writer.IndentLevel = 0;
         foreach (char ch in @"Line6") {
            writer.Write(ch);
         }

         //ASSERT
         Assert.AreEqual(expected, sb.ToString());
      }
   }
}
