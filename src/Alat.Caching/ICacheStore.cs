using System.Collections.Generic;

namespace Alat.Caching {
   public interface ICacheStore {
      bool Contains(string key);
      bool Any();

      CacheItem<TData> Retrieve<TData>(string key);
      
      void Store<TData>(CacheItem<TData> item);

      void Remove(string key);
      void Remove(IEnumerable<string> keys);
      void RemoveAll();
      void RemoveExpired();

      long GetSize();
   }
}