using System;
using System.Collections.Generic;

namespace Alat.Http.Tests.Mocks {
   public class CacheMock : Caching.ICache {
      private Dictionary<string, CacheItem> Items = new Dictionary<string, CacheItem>();

      public bool Contains(string key) {
         return Items.ContainsKey(key);
      }

      public void Remove(string key) {
         Items.Remove(key);
      }

      public void Remove(IEnumerable<string> keys) {
         foreach(var key in keys) {
            Items.Remove(key);
         }
      }

      public T Retrieve<T>(string key) {
         T data = default(T);
         if (Items.ContainsKey(key)) {
            data = (T)Items[key].Data;
         }

         return data;
      }

      public void Store<TData>(string key, TData data, TimeSpan expireIn, string tag = null) {
         Items.Add(key, new CacheItem() {
            Key = key,
            Tag = tag,
            Data = data,
            ExpireOn = DateTime.UtcNow.Add(expireIn)
         });
      }

      private class CacheItem {
         public string Key { get; set; }
         public string Tag { get; set; }
         public DateTime ExpireOn { get; set; }
         public object Data { get; set; }
      }
   }
}
