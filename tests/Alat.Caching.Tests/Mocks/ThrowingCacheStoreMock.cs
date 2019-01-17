using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Caching.Tests.Mocks {
   internal class ThrowingCacheStoreMock : CacheStore {
      private readonly IEnumerable<CacheItem> items;

      public bool CleanCalled { get; set; }
      public bool RemoveAllCalled { get; set; }

      public List<string> RemovedKeys { get; } = new List<string>();

      public ThrowingCacheStoreMock(IEnumerable<CacheItem> items) {
         this.items = items;
      }

      public void Add<T>(string key, T data, string tag, DateTime dateTime) {
         throw new AddCalledException(Tuple.Create(key, data.ToString(), tag, dateTime));
      }

      public bool Any() {
         return items.Any();
      }

      public void Clean() {
         throw new CleanCalledException();
      }

      public bool Contains(string key) {
         return items.Any(i => i.Key == key);
      }

      public CacheItem Find<T>(string key) {
         return items.FirstOrDefault(i => i.Key == key);
      }

      public string FindTag(string key) {
         return items.FirstOrDefault(i => i.Key == key).Tag;
      }

      public long GetSize() {
         return 42;
      }

      public void Remove(string[] key) {
         throw new RemoveCalledException(key);
      }

      public void RemoveAll() {
         throw new RemoveAllCalledException();
      }

      public void Reset(string key, DateTime dateTime) {
         throw new ResetCalledException(Tuple.Create(key, dateTime));
      }
   }
}
