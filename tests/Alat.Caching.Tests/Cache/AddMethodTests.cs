using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class AddMethodTests : MethodTests {
      private string Key { get; } = "key";
      private string Data { get; } = "data";
      private TimeSpan Expiration { get; } = TimeSpan.FromMinutes(5);
      private string Tags { get; } = "tag1,tag2";

      [Fact]
      public void Default() {
         var ex = Assert.Throws<Mocks.MethodCalledException>(() => Cache.Store(Key, Data, Expiration, Tags));

         Assert.Equal("Store", ex.Name);
         Assert.Equal(Key, ex.Parameters[0]);
         Assert.Equal(Data, ex.Parameters[1]);
         Assert.Equal(Tags, ex.Parameters[2]);
         Assert.InRange((DateTime)ex.Parameters[3], DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(6));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Store(null, Data, Expiration, Tags)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Store("", Data, Expiration, Tags)
         );
      }

      [Fact]
      public void NullData() {
         string nullData = null;
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Store(Key, nullData, Expiration, Tags)
         );
      }

      [Fact]
      public void NullTags() {
         var ex = Assert.Throws<Mocks.MethodCalledException>(() =>
            Cache.Store(Key, Data, Expiration, null)
         );

         Assert.Equal("Store", ex.Name);
         Assert.Equal(Key, ex.Parameters[0]);
         Assert.Equal(Data, ex.Parameters[1]);
         Assert.Null(ex.Parameters[2]);
         Assert.NotInRange((DateTime)ex.Parameters[3], DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(5));
      }

      [Fact]
      public void TagsNotPassed() {
         var ex = Assert.Throws<Mocks.MethodCalledException>(() =>
            Cache.Store(Key, Data, Expiration)
         );

         Assert.Equal("Store", ex.Name);
         Assert.Equal(Key, ex.Parameters[0]);
         Assert.Equal(Data, ex.Parameters[1]);
         Assert.Null(ex.Parameters[2]);
         Assert.NotInRange((DateTime)ex.Parameters[3], DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(5));
      }

      [Fact]
      public void NegativeExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Store(Key, Data, new TimeSpan(-1), Tags)
         );
      }

      [Fact]
      public void ZeroExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Store(Key, Data, TimeSpan.Zero, Tags)
         );
      }
   }
}
