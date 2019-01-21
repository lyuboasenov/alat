using System;

namespace Alat.Caching.Tests.CacheStore {
   public class MethodTests {
      protected CacheItem<TData> CreateCacheItem<TData>(string key, TData data, string tag, DateTime dateTime) {
         return new CacheItem<TData>() {
            Meta = new CacheItemMeta() {
               Key = key,
               Tag = tag,
               ExpirationDate = dateTime
            },
            Data = data
         };
      }
   }
}
