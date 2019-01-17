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
         
         Assert.Equal(ExistingKeyValue, Cache.FindData<string>(ExistingKey));
         Assert.Null(Cache.FindData<string>(NonExistingKey));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() => 
            Cache.FindData<string>(NullValuedKey)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.FindData<string>(EmptyValuedKey)
         );
      }


      [Fact]
      public void WrongType() {
         Assert.Throws<InvalidCastException>(() =>
            Cache.FindData<DateTime>(ExistingKey)
         );
      }
   }
}
