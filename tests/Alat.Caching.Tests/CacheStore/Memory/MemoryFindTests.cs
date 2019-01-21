using Alat.Caching.Memory;

namespace Alat.Caching.Tests.CacheStore.Memory {
   public class MemoryFindTests : FindTests {
      protected override Caching.ICacheStore CreateCacheStore() {
         return new ReferencingMemoryCacheStore();
      }
   }
}
