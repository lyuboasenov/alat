using NUnit.Framework;
using System.Collections.Generic;

namespace Alat.Utils.Tests {
   [TestFixture]
   public class ObjectExtensionsTests {
      public class DummyClass {
         public int Prop1 { get; set; }
         private string Prop2 { get; set; }
         private string[] Prop3 { get; set; }
         public List<int> Prop4 { get; set; }

         public DummyClass(int prop1, string prop2, string[] prop3, List<int> prop4) {
            Prop1 = prop1;
            Prop2 = prop2;
            Prop3 = prop3;
            Prop4 = prop4;
         }

         public override bool Equals(object obj) {
            return this.ReflectionEquals(obj);
         }
      }

      [Test]
      public void Equals_ComplexObjects_ReturnsTrue() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsTrue(result);
      }

      [Test]
      public void Equals_SameComplexObject_ReturnsTrue() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj1);


         // ASSERT
         Assert.IsTrue(result);
      }

      [Test]
      public void Equals_ComplexObjectAndNull_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, null);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_NullAndComplexObject_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(null, obj1);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_DiffComplexObj1_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(2, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_DiffComplexObj2_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str1", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_DiffComplexObj3_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str", new[] { "str", "str2" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_DiffComplexObj4_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str", new[] { "str1" }, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }

      [Test]
      public void Equals_DiffComplexObj5_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str", null, new List<int> { 1, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }



      [Test]
      public void Equals_DiffComplexObj6_ReturnsFalse() {
         // ARRANGE
         var obj1 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 1, 2, 3 });
         var obj2 = new DummyClass(1, "str", new[] { "str1", "str2" }, new List<int> { 2, 2, 3 });


         // ACT
         var result = Equals(obj1, obj2);


         // ASSERT
         Assert.IsFalse(result);
      }

   }
}
