﻿using Alat.Caching.Memory;

namespace Alat.Caching.Tests.CacheStore.Memory {
   public class MemoryAnyTests : AnyTests {
      protected override Caching.ICacheStore CreateCacheStore() {
         return new ReferencingMemoryCacheStore();
      }
   }
}
