using System;
using System.Collections.Generic;

namespace Alat.Caching {
   public interface ICache {
      void Clean();
      bool Contains(string key);
      long GetCacheSize();
      void Remove(IEnumerable<string> keys);
      void Remove(string key);
      void Remove(string key, string tag);
      void RemoveAll();
      void RemoveExired();
      T Retrieve<T>(string key);
      T Retrieve<T>(string key, string tag);
      void Store<TData>(string key, TData data);
      void Store<TData>(string key, TData data, string tag = null);
      void Store<TData>(string key, TData data, TimeSpan expireIn);
      void Store<TData>(string key, TData data, TimeSpan expireIn, string tag = null);
   }
}