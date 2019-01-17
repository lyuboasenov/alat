using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Caching.Memory {
   public class MemoryCacheStore : CacheStore {
      private readonly object lockObj = new object();
      private IDictionary<string, MemoryCacheItem> Items { get; } = new Dictionary<string, MemoryCacheItem>();

      public CacheItem Find<T>(string key) {
         CacheItem result = null;
         if (Contains(key)) {
            var item = Items[key];
            result = new MemoryCacheItem() {
               Key = item.Key,
               Tag = item.Tag,
               Data = item.Data,
               ExpirationDate = item.ExpirationDate
            };
         }

         return result;
      }

      public string FindTag(string key) {
         return Contains(key) ? Items[key].Tag : string.Empty;
      }

      public void Add<T>(string key, T data, string tag, DateTime expirationDate) {
         lock (lockObj) {
            var item = new MemoryCacheItem() {
               Key = key,
               ExpirationDate = expirationDate,
               Tag = tag,
               Data = data
            };

            Items[key] = item;
         }
      }

      public void Clean() {
         lock (lockObj) {
            var keysToRemove = Items.
               Where(pair => pair.Value.ExpirationDate < DateTime.Now).
               Select(p => p.Key).ToArray();

            foreach (var key in keysToRemove) {
               Items.Remove(key);
            }
         }
      }

      public bool Contains(string key) {
         return Items.ContainsKey(key);
      }

      public bool Any() {
         return Items.Count > 0;
      }

      public CacheItem Find(string key) {
         CacheItem result = null;
         if(Items.ContainsKey(key)) {
            result = Items[key];
         }

         return result;
      }

      public void Reset(string key, DateTime dateTime) {
         if(Items.ContainsKey(key)) {
            lock (lockObj) {
               Items[key].ExpirationDate = dateTime;
            }
         }
      }

      public void Remove(string[] keys) {
         lock (lockObj) {
            foreach(var key in keys) {
               if (Items.ContainsKey(key)) {
                  Items.Remove(key);
               }
            }
         }
      }

      public void RemoveAll() {
         lock (lockObj) {
            Items.Clear();
         }
      }

      public long GetSize() {
         return 1000000000;
      }
   }
}
