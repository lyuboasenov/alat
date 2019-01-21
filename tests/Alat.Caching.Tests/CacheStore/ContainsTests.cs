using System;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class ContainsTests : MethodTests {
      private Caching.ICacheStore CacheStore { get; }

      private string ExistingKey { get; } = "existing-key";
      private string ExistingData { get; } = "existing-data";
      private string ExistingTags { get; } = "existing-tags";

      private string NonExistingKey { get; } = "non-existing-key";
      private string EmptyKey { get; } = "";
      private string NullKey { get; } = null;

      private DateTime ExisingExpirationDate { get; } = DateTime.UtcNow.AddMinutes(1);

      protected ContainsTests() {
         CacheStore = CreateCacheStore();

         CacheStore.Store(
            CreateCacheItem(ExistingKey, ExistingData, ExistingTags, ExisingExpirationDate));
      }

      [Fact]
      public void RegularCall() {
         Assert.True(CacheStore.Contains(ExistingKey));
         Assert.False(CacheStore.Contains(NonExistingKey));
      }

      [Fact]
      public void NullKeyCall() {
         Assert.Throws<ArgumentNullException>(() => CacheStore.Contains(NullKey));
      }

      [Fact]
      public void EmptyKeyCall() {
         Assert.Throws<ArgumentException>(() => CacheStore.Contains(EmptyKey));
      }

      protected abstract Caching.ICacheStore CreateCacheStore();
   }
}
