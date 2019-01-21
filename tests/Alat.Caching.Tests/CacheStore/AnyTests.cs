using System;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class AnyTests : MethodTests {

      private string ExistingKey { get; } = "existing-key";
      private string ExistingData { get; } = "existing-data";
      private string ExistingTags { get; } = "existing-tags";

      private DateTime ExisingExpirationDate { get; } = DateTime.UtcNow.AddMinutes(1);

      protected AnyTests() {
      }

      [Fact]
      public void EmptyStoreCall() {
         var cacheStore = CreateCacheStore();
         Assert.False(cacheStore.Any());
         
      }

      [Fact]
      public void NotEmptyStoreCall() {
         var cacheStore = CreateCacheStore();

         cacheStore.Store(
            CreateCacheItem(ExistingKey, ExistingData, ExistingTags, ExisingExpirationDate));

         Assert.True(cacheStore.Any());
      }

      protected abstract Caching.ICacheStore CreateCacheStore();
   }
}
