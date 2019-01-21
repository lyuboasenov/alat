using Alat.Caching.Tests.Mocks;
using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class FindMethodTests : MethodTests {
      private string ExistingKey { get; } = "key1";
      private string ExistingKeyValue { get; } = "Dummy data 1";
      private string NullValuedKey { get; } = null;
      private string EmptyValuedKey { get; } = "";
      private string NonExistingKey { get; } = "keyJ";
      

      [Fact]
      public void Default() {
         Testing.Assert.MethodCalled(() => Cache.Retrieve<string>(ExistingKey),
            "Retrieve", ExistingKey);
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() => 
            Cache.Retrieve<string>(NullValuedKey)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Retrieve<string>(EmptyValuedKey)
         );
      }
   }
}
