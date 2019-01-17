using Alat.Caching.Tests.Mocks;
using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class ContainsMethodTests : MethodTests {
      private string ExistingKey { get; } = "key1";
      private string NonExistingKey { get; } = "keyJ";
      

      [Fact]
      public void Default() {
         Assert.True(Cache.Contains(ExistingKey));
         Assert.False(Cache.Contains(NonExistingKey));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Contains(null)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Contains("")
         );
      }
   }
}
