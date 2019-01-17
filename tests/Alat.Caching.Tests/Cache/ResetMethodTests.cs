using Alat.Caching.Tests.Mocks;
using System;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class ResetMethodTests : MethodTests {
      private string Key { get; } = "key";
      private TimeSpan Expiration { get; } = TimeSpan.FromMinutes(5);
      private TimeSpan NegativeExpirationValue { get; } = new TimeSpan(-1);
      private TimeSpan ZeroExpirationValue { get; } = TimeSpan.Zero;

      [Fact]
      public void Default() {
         var ex = Assert.Throws<ResetCalledException>(() =>
            Cache.Reset(Key, Expiration)
         );

         Assert.Equal(Key, ex.Parameters.Item1);
         Assert.InRange(ex.Parameters.Item2, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(6));
      }

      [Fact]
      public void NullKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Reset(null, Expiration)
         );
      }

      [Fact]
      public void EmptyKey() {
         Assert.Throws<ArgumentNullException>(() =>
            Cache.Reset("", Expiration)
         );
      }

      [Fact]
      public void NegativeExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Reset(Key, NegativeExpirationValue)
         );
      }

      [Fact]
      public void ZeroExpiration() {
         Assert.Throws<ArgumentException>(() =>
            Cache.Reset(Key, ZeroExpirationValue)
         );
      }
   }
}
