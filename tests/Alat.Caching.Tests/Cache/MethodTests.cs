using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Cache {
   public abstract class MethodTests {
      protected Caching.ICacheStore Store { get; }

      protected IEnumerable<CacheItem<string>> Items { get; }

      protected Caching.Cache Cache { get; }

      protected MethodTests() {
         Items = Mocks.CacheItemMock.GetMockItems();

         Store = new Mocks.ThrowingCacheStoreMock();

         Cache = new Caching.Cache(Store);
      }
   }
}
