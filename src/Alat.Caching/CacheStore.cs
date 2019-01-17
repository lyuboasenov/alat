using System;

namespace Alat.Caching {
   public interface CacheStore {
      bool Contains(string key);

      CacheItem Find<T>(string key);
      string FindTag(string key);

      void Add<T>(string key, T data, string tag, DateTime dateTime);
      void Remove(string[] key);
      void RemoveAll();

      void Reset(string key, DateTime dateTime);

      void Clean();

      bool Any();

      long GetSize();
   }
}