using System;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class RemoveTests : MethodTests {
      private Caching.ICacheStore CacheStore { get; }

      private string ExistingKey1 { get; } = "existing-key-1";
      private string ExistingData1 { get; } = "existing-data-1";
      private string ExistingTags1 { get; } = "existing-tags-1";

      private string ExistingKey2 { get; } = "existing-key-2";
      private string ExistingData2 { get; } = "existing-data-2";
      private string ExistingTags2 { get; } = "existing-tags-2";

      private string ExistingKey3 { get; } = "existing-key-3";
      private string ExistingData3 { get; } = "existing-data-3";
      private string ExistingTags3 { get; } = "existing-tags-3";

      private string NonExistingKey { get; } = "non-existing-key";
      private string EmptyKey { get; } = "";
      private string NullKey { get; } = null;

      private DateTime ExisingExpirationDate { get; } = DateTime.UtcNow.AddMinutes(1);

      protected RemoveTests() {
         CacheStore = CreateCacheStore();

         CacheStore.Store(
            CreateCacheItem(ExistingKey1, ExistingData1, ExistingTags1, ExisingExpirationDate));
         CacheStore.Store(
            CreateCacheItem(ExistingKey2, ExistingData2, ExistingTags2, ExisingExpirationDate));
         CacheStore.Store(
            CreateCacheItem(ExistingKey3, ExistingData3, ExistingTags3, ExisingExpirationDate));
      }

      [Fact]
      public void RemoveSingleItemCall() {
         CacheStore.Remove(new string[] { ExistingKey1 });

         Assert.False(CacheStore.Contains(ExistingKey1));
         Assert.True(CacheStore.Contains(ExistingKey2));
         Assert.True(CacheStore.Contains(ExistingKey3));
      }

      [Fact]
      public void RemoveMultipleItemCall() {
         CacheStore.Remove(new string[] { ExistingKey1, ExistingKey2 });

         Assert.False(CacheStore.Contains(ExistingKey1));
         Assert.False(CacheStore.Contains(ExistingKey2));
         Assert.True(CacheStore.Contains(ExistingKey3));
      }

      [Fact]
      public void NullKeyCall() {
         Assert.Throws<ArgumentNullException>(() => CacheStore.Remove(new[] { NullKey }));
      }

      [Fact]
      public void NullContainingKeyCall() {
         Assert.Throws<ArgumentNullException>(() => CacheStore.Remove(new[] { ExistingKey1, NullKey, ExistingKey2 }));
      }

      [Fact]
      public void EmptyKeyCall() {
         Assert.Throws<ArgumentException>(() => CacheStore.Remove(new[] { EmptyKey }));
      }

      [Fact]
      public void EmptyContainingKeyCall() {
         Assert.Throws<ArgumentException>(() => CacheStore.Remove(new[] { ExistingKey1, EmptyKey, ExistingKey2 }));
      }

      protected abstract Caching.ICacheStore CreateCacheStore();
   }
}
