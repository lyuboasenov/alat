using Alat.Caching.Tests.Mocks;
using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class FindTagMethodTests : MethodTests {
      private string ExistingKey { get; } = "key1";
      private string ExistingKeyNoTag { get; } = "key6";
      private string ExistingKeyTag { get; } = "tag11,tag12,tag13";
      private string NullValuedKey { get; } = null;
      private string EmptyValuedKey { get; } = "";
      private string NonExistingKey { get; } = "keyJ";
      

      [Fact]
      public void Default() {
         Assert.Equal(ExistingKeyTag, Cache.FindTag(ExistingKey));
         Assert.Null(Cache.FindTag(ExistingKeyNoTag));
         Assert.Throws<NullReferenceException>(() => Cache.FindTag(NonExistingKey));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() => 
            Cache.FindTag(NullValuedKey)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.FindTag(EmptyValuedKey)
         );
      }
   }
}
