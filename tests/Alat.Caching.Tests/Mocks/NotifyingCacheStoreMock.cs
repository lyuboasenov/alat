using Alat.Testing.Mocks;
using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Mocks {
   internal class NotifyingCacheStoreMock : NotifyingMock, ICacheStore {

      public NotifyingCacheStoreMock(IEnumerable<CacheItem<string>> items) { }

      public void Store<TData>(CacheItem<TData> item) {
         MethodCalled(nameof(Store), item.Meta.Key, item.Data.ToString(), item.Meta.Tag, item.Meta.ExpirationDate);
      }

      public CacheItem<TData> Retrieve<TData>(string key) {
         MethodCalled(nameof(Retrieve), key);
         return default(CacheItem<TData>);
      }

      public bool Any() {
         MethodCalled(nameof(Any));
         return true;
      }

      public bool Contains(string key) {
         MethodCalled(nameof(Contains), key);
         return default(bool);
      }

      public long GetSize() {
         MethodCalled(nameof(GetSize));
         return default(long);
      }

      public void Remove(string key) {
         MethodCalled(nameof(Remove), key);
      }

      public void Remove(IEnumerable<string> key) {
         MethodCalled(nameof(Remove), key);
      }

      public void RemoveAll() {
         MethodCalled(nameof(RemoveAll));
      }

      public void RemoveExpired() {
         MethodCalled(nameof(RemoveExpired));
      }

      public void Reset(string key, DateTime dateTime) {
         MethodCalled(nameof(Reset), key, dateTime);
      }
   }
}
