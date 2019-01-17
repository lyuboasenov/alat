using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Cache {
   public abstract class MethodTests {
      protected CacheStore Store { get; }
      protected CacheStore EmptyStore { get; }

      protected IEnumerable<CacheItem> Items { get; }

      protected Caching.Cache Cache { get; }
      protected Caching.Cache EmptyCache { get; }

      protected MethodTests() {
         Items = Mocks.CacheItemMock.GetMockItems();

         Store = new Mocks.ThrowingCacheStoreMock(Items);
         EmptyStore = new Mocks.ThrowingCacheStoreMock(Array.Empty<CacheItem>());

         Cache = new Impl.Cache(Store);
         EmptyCache = new Impl.Cache(EmptyStore);
      }
   }
}
