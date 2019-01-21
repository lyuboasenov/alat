using System;
using System.Threading;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class CreateCacheTests {
      private Caching.ICacheStore Store { get; }

      public CreateCacheTests() {
         Store = new Mocks.ThrowingCacheStoreMock();
      }

      [Fact]
      public void Default() {
         Assert.NotNull(new Caching.Cache(Store));
      }

      [Fact]
      public void NullStore() {
         Assert.Throws<ArgumentNullException>(() => new Caching.Cache(null));
      }

      [Fact]
      public void NegativeCleanInterval() {
         Assert.NotNull(new Caching.Cache(Store, new TimeSpan(-1)));
      }

      [Fact]
      public void ZeroCleanInterval() {
         Assert.NotNull(new Caching.Cache(Store, TimeSpan.Zero));
      }

      [Fact]
      public void MaxCleanInterval() {
         Assert.NotNull(new Caching.Cache(Store, TimeSpan.MaxValue));
      }

      [Fact]
      public void InfiniteCleanInterval() {
         Assert.NotNull(new Caching.Cache(Store, Timeout.InfiniteTimeSpan));
      }
   }
}
