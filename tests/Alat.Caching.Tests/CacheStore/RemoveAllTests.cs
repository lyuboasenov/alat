using System;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class RemoveAllTests : MethodTests {
      private Caching.ICacheStore CacheStore { get; }

      private string ExistingKey1 { get; } = "existing-key-1";
      private string ExistingData1 { get; } = "existing-data-1";
      private string ExistingTags1 { get; } = "existing-tags-1";

      private string ExistingKey2 { get; } = "existing-key-2";
      private string ExistingData2 { get; } = "existing-data-2";
      private string ExistingTags2 { get; } = "existing-tags-2";

      private DateTime ExisingExpirationDate { get; } = DateTime.UtcNow.AddMinutes(1);

      protected RemoveAllTests() {
         CacheStore = CreateCacheStore();
      }

      [Fact]
      public void RemoveAllCall() {
         AddItems();

         CacheStore.RemoveAll();

         Assert.False(CacheStore.Contains(ExistingKey1));
         Assert.False(CacheStore.Contains(ExistingKey2));
      }

      [Fact]
      public void RemoveAllOnEmptyStoreCall() {
         AddItems();

         CacheStore.RemoveAll();
         CacheStore.RemoveAll();

         Assert.False(CacheStore.Contains(ExistingKey1));
         Assert.False(CacheStore.Contains(ExistingKey2));
      }

      protected abstract ICacheStore CreateCacheStore();

      private void AddItems() {
         CacheStore.Store(
            CreateCacheItem(ExistingKey1, ExistingData1, ExistingTags1, ExisingExpirationDate));
         CacheStore.Store(
            CreateCacheItem(ExistingKey2, ExistingData2, ExistingTags2, ExisingExpirationDate));
      }
   }
}
