using System;
using System.Collections.Generic;

namespace Alat.Http.Caching {
   public interface ICache {
      bool Contains(string key);
      void Remove(string key);
      void Remove(IEnumerable<string> keys);

      T Retrieve<T>(string key);

      void Store<TData>(string key, TData data, TimeSpan expireIn, string tag = null);
   }
}
