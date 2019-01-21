using Alat.Caching.Memory;

namespace Alat.Caching.Tests.CacheStore.Memory {
   public class MemoryRemoveTests : RemoveTests {
      protected override Caching.ICacheStore CreateCacheStore() {
         return new ReferencingMemoryCacheStore();
      }
   }
}
