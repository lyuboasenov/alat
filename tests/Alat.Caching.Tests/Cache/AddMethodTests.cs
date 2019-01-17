using Alat.Caching.Tests.Mocks;
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
         var ex = Assert.Throws<AddCalledException>(() =>
            Cache.Add(Key, Data, Expiration, Tags)
         );

         Assert.Equal(Key, ex.Parameters.Item1);
         Assert.Equal(Data, ex.Parameters.Item2);
         Assert.Equal(Tags, ex.Parameters.Item3);
         Assert.InRange(ex.Parameters.Item4, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(6));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Add(null, Data, Expiration, Tags)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Add("", Data, Expiration, Tags)
         );
      }

      [Fact]
      public void NullData() {
         string nullData = null;
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Add(Key, nullData, Expiration, Tags)
         );
      }

      [Fact]
      public void NullTags() {
         var ex = Assert.Throws<AddCalledException>(() =>
            Cache.Add(Key, Data, Expiration, null)
         );

         Assert.Equal(Key, ex.Parameters.Item1);
         Assert.Equal(Data, ex.Parameters.Item2);
         Assert.Null(ex.Parameters.Item3);
         Assert.NotInRange(ex.Parameters.Item4, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(5));
      }

      [Fact]
      public void TagsNotPassed() {
         var ex = Assert.Throws<AddCalledException>(() =>
            Cache.Add(Key, Data, Expiration)
         );

         Assert.Equal(Key, ex.Parameters.Item1);
         Assert.Equal(Data, ex.Parameters.Item2);
         Assert.Null(ex.Parameters.Item3);
         Assert.NotInRange(ex.Parameters.Item4, DateTime.Now.AddMinutes(-1), DateTime.Now.AddMinutes(5));
      }

      [Fact]
      public void NegativeExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Add(Key, Data, new TimeSpan(-1), Tags)
         );
      }

      [Fact]
      public void ZeroExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Add(Key, Data, TimeSpan.Zero, Tags)
         );
      }
   }
}
