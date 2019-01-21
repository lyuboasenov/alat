using System;
using System.Threading;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class CleanTests : MethodTests {
      private Caching.ICacheStore CacheStore { get; }

      private string ExistingKey1 { get; } = "existing-key-1";
      private string ExistingData1 { get; } = "existing-data-1";
      private string ExistingTags1 { get; } = "existing-tags-1";
      private DateTime ExisingExpirationDate1 { get; } = DateTime.UtcNow.AddMinutes(1);

      private string ExistingKey2 { get; } = "existing-key-2";
      private string ExistingData2 { get; } = "existing-data-2";
      private string ExistingTags2 { get; } = "existing-tags-2";
      private DateTime ExisingExpirationDate2 { get; } = DateTime.UtcNow.AddMilliseconds(500);

      protected CleanTests() {
         CacheStore = CreateCacheStore();
      }

      [Fact]
      public void CleanCall() {
         AddItems();

         Thread.Sleep(500);

         CacheStore.RemoveExpired();

         Assert.True(CacheStore.Contains(ExistingKey1));
         Assert.False(CacheStore.Contains(ExistingKey2));
      }

      protected abstract Caching.ICacheStore CreateCacheStore();

      private void AddItems() {
         CacheStore.Store(
            CreateCacheItem(ExistingKey1, ExistingData1, ExistingTags1, ExisingExpirationDate1));
         CacheStore.Store(
            CreateCacheItem(ExistingKey2, ExistingData2, ExistingTags2, ExisingExpirationDate2));
      }
   }
}
