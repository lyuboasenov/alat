using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Caching.Tests.Mocks {
   internal class ThrowingCacheStoreMock : ThrowingMock, Caching.ICacheStore {
      private bool AnyTrows { get; }

      public ThrowingCacheStoreMock(bool anyTrows = false) {
         AnyTrows = anyTrows;
      }

      public void Store<TData>(CacheItem<TData> item) {
         MethodCalled(nameof(Store), item.Meta.Key, item.Data.ToString(), item.Meta.Tag, item.Meta.ExpirationDate);
      }

      public bool Any() {
         if (AnyTrows) {
            MethodCalled(nameof(Any));
         }

         return true;
      }

      public bool Contains(string key) {
         MethodCalled(nameof(Contains), key);
         return false;
      }

      public long GetSize() {
         return 42;
      }
      public CacheItem<TData> Retrieve<TData>(string key) {
         MethodCalled(nameof(Retrieve), key);
         return default(CacheItem<TData>);
      }

      public void Remove(string key) {
         MethodCalled(nameof(Remove), key);
      }

      public void Remove(IEnumerable<string> keys) {
         MethodCalled(nameof(Remove), keys.ToArray());
      }

      public void RemoveExpired() {
         MethodCalled(nameof(RemoveExpired));
      }

      public void RemoveAll() {
         MethodCalled(nameof(RemoveAll));
      }
   }
}
