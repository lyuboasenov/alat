using Alat.Caching.Memory;

namespace Alat.Caching.Tests.CacheStore.Memory {
   public class MemoryAddTests : AddTests {
      protected override Caching.ICacheStore CreateCacheStore() {
         return new ReferencingMemoryCacheStore();
      }
   }
}
