using NUnit.Framework;

namespace Alat.Utils.Tests {
   [TestFixture]
   public class StringExtensionsTests {
      [Test]
      public void BreakLineIntoLengthRestrictedLines_LongerThanMaxLength_ReturnsMultiLineString() {
         // ARRANGE
         var line = "asdfghjkl";
         var expected = @"asdf
ghjk
l";

         // ACT
         var result = Alat.PSSp.Functionality.Utils.StringExtensions.BreakLineIntoLengthRestrictedLines(line, 4);

         //ASSERT
         Assert.AreEqual(expected, result);
      }

      [Test]
      public void BreakLineIntoLengthRestrictedLines_EqualThanMaxLength_ReturnsSingleLineString() {
         // ARRANGE
         var line = "asdfghjkl";
         var expected = @"asdfghjkl";

         // ACT
         var result = Alat.PSSp.Functionality.Utils.StringExtensions.BreakLineIntoLengthRestrictedLines(line, 9);

         //ASSERT
         Assert.AreEqual(expected, result);
      }


      [Test]
      public void BreakMultiLineIntoLengthRestrictedLines_TwoLongerThanMaxLength_ReturnsMultiLineString() {
         // ARRANGE
         var text = @"asdfghjkl
asdfghjkl";
         var expected = @"asdf
ghjk
l
asdf
ghjk
l";

         // ACT
         var result = Alat.PSSp.Functionality.Utils.StringExtensions.BreakMultiLineIntoLengthRestrictedLines(text, 4);

         //ASSERT
         Assert.AreEqual(expected, result);
      }


      [Test]
      public void BreakMultiLineIntoLengthRestrictedLines_TwoEqualThanMaxLength_ReturnsMultiLineString() {
         // ARRANGE
         var text = @"asdfghjkl
asdfghjkl";
         var expected = @"asdfghjkl
asdfghjkl";

         // ACT
         var result = Alat.PSSp.Functionality.Utils.StringExtensions.BreakMultiLineIntoLengthRestrictedLines(text, 9);

         //ASSERT
         Assert.AreEqual(expected, result);
      }
   }
}
