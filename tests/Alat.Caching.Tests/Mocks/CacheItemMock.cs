using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Mocks {
   class CacheItemMock : CacheItem<string> {

      public static IEnumerable<CacheItem<string>> GetMockItems() {
         for (int i = 1; i <= 5; i++) {
            yield return new CacheItemMock() {
               Meta = new CacheItemMeta() {
                  Key = $"key{i}",
                  Tag = $"tag{i}1,tag{i}2,tag{i}3",
                  ExpirationDate = DateTime.Now.AddMinutes(5)
               },
               Data = $"Dummy data {i}",
               
            };
         }

         for (int i = 6; i <= 10; i++) {
            yield return new CacheItemMock() {
               Meta = new CacheItemMeta() {
                  Key = $"key{i}",
                  Tag = null,
                  ExpirationDate = DateTime.Now.AddMinutes(5)
               },
               Data = $"Dummy data {i}",
            };
         }
      }
   }
}
