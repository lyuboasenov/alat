using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Caching.Memory {
   internal class ReferencingMemoryCacheStore : ICacheStore {
      private readonly object lockObj = new object();
      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();
      private IDictionary<string, ReferencingMemoryCacheItem> Items { get; } = new Dictionary<string, ReferencingMemoryCacheItem>();

      public CacheItem<TData> Retrieve<TData>(string key) {
         KeyValidator.ValidateKey(key);

         CacheItem<TData> result = null;
         if (Contains(key)) {
            var item = Items[key];
            result = new CacheItem<TData>() {
               Meta = Items[key].Meta,
               Data = (TData)Items[key].Data
            };
         }

         return result;
      }

      public void Store<TData>(CacheItem<TData> item) {
         KeyValidator.ValidateKey(item.Meta.Key);

         if (null == item.Data) {
            throw new ArgumentNullException(nameof(item.Data));
         }

         lock (lockObj) {
            var memoryItem = new ReferencingMemoryCacheItem() {
               Meta = item.Meta,
               Data = item.Data
            };

            Items[item.Meta.Key] = memoryItem;
         }
      }

      public void Remove(string key) {
         KeyValidator.ValidateKey(key);

         Remove(new[] { key });
      }

      public void Remove(IEnumerable<string> keys) {
         KeyValidator.ValidateKeys(keys);

         lock (lockObj) {
            foreach (var key in keys) {
               if (Items.ContainsKey(key)) {
                  Items.Remove(key);
               }
            }
         }
      }

      public void RemoveExpired() {
         lock (lockObj) {
            var keysToRemove = Items.
               Where(pair => pair.Value.Meta.ExpirationDate < DateTime.UtcNow).
               Select(p => p.Key).ToArray();

            foreach (var key in keysToRemove) {
               Items.Remove(key);
            }
         }
      }

      public bool Contains(string key) {
         KeyValidator.ValidateKey(key);

         return Items.ContainsKey(key);
      }

      public bool Any() {
         return Items.Count > 0;
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
