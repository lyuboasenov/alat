using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Mocks {
   class CacheItemMock : CacheItem {
      public string Key { get; set; }
      public string Tag { get; set; }
      public object Data { get; set; }
      public DateTime ExpirationDate { get; set; }

      public static IEnumerable<CacheItem> GetMockItems() {
         for (int i = 1; i <= 5; i++) {
            yield return new CacheItemMock() {
               Key = $"key{i}",
               Tag = $"tag{i}1,tag{i}2,tag{i}3",
               Data = $"Dummy data {i}",
               ExpirationDate = DateTime.Now.AddMinutes(5)
            };
         }

         for (int i = 6; i <= 10; i++) {
            yield return new CacheItemMock() {
               Key = $"key{i}",
               Tag = null,
               Data = $"Dummy data {i}",
               ExpirationDate = DateTime.Now.AddMinutes(5)
            };
         }
      }
   }
}
