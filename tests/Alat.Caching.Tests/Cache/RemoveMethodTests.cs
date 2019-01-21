using Alat.Caching.Tests.Mocks;
using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class RemoveMethodTests : MethodTests {
      private string[] ValidKeys1 { get; } = new string[] { "key1", "key2", "key3" };
      private string ValidKeys2 { get; } = "key1";
      private string[] InvalidKeys1 { get; } = new string[] { "key1", "", "key3" };
      private string[] InvalidKeys2 { get; } = new string[] { "key1", null, "key3" };
      private string[] InvalidKeys3 { get; } = Array.Empty<string>();
      private string[] InvalidKeys4 { get; } = null;
      private string InvalidKeys5 { get; } = null;
      private string InvalidKeys6 { get; } = "";
      

      [Fact]
      public void Default() {
         Testing.Assert.MethodCalled(() => Cache.Remove(ValidKeys1), "Remove", ValidKeys1);
      }

      [Fact]
      public void ArrayContainingEmptyKey() {
         Assert.Throws<ArgumentException>(() => Cache.Remove(InvalidKeys1));
      }

      [Fact]
      public void ArrayContainingNullKey() {
         Assert.Throws<ArgumentNullException>(() => Cache.Remove(InvalidKeys2));
      }

      [Fact]
      public void EmptyArray() {
         Assert.Throws<ArgumentException>(() => Cache.Remove(InvalidKeys3));
      }

      [Fact]
      public void NullArray() {
         Assert.Throws<ArgumentNullException>(() => Cache.Remove(InvalidKeys4));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() => Cache.Remove(InvalidKeys5));
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentException>(() => Cache.Remove(InvalidKeys6));
      }
   }
}
