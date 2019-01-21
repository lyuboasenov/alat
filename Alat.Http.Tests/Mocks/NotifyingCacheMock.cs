using Alat.Http.Caching;
using Alat.Testing.Mocks;
using System;
using System.Collections.Generic;

namespace Alat.Http.Tests.Mocks {
   public class NotifyingCacheMock : NotifyingMock, ICache {
      public bool Contains(string key) {
         MethodCalled(nameof(Contains), key);
         return default(bool);
      }

      public void Remove(string key) {
         MethodCalled(nameof(Remove), key);
      }

      public void Remove(IEnumerable<string> keys) {
         MethodCalled(nameof(Remove), keys);
      }

      public T Retrieve<T>(string key) {
         MethodCalled(nameof(Retrieve), key);
         return default(T);
      }

      public void Store<TData>(string key, TData data, TimeSpan expireIn, string tag = null) {
         MethodCalled(nameof(Store), key, data, expireIn, tag);
      }
   }
}
