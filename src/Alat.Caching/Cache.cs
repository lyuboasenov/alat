using System;

namespace Alat.Caching {
   public interface Cache {
      bool Contains(string key);
      T FindData<T>(string key);
      string FindTag(string key);

      void Add<T>(string key, T data, TimeSpan expireIn, string tag = null);
      void Remove(params string[] key);

      void Reset(string key, TimeSpan expireIn);

      void Clean();
      void Invalidate();

      long GetCacheSize();
   }
}
