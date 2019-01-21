using Alat.Caching.Memory;

namespace Alat.Caching.Tests.CacheStore.Memory {
   public class MemoryRemoveAllTests : RemoveAllTests {
      protected override Caching.ICacheStore CreateCacheStore() {
         return new ReferencingMemoryCacheStore();
      }
   }
}
