using System;

namespace Alat.Caching.Memory {
   public class MemoryCacheItem : CacheItem {
      public string Key { get; set; }
      public string Tag { get; set; }
      public DateTime ExpirationDate { get; set; }
      public object Data { get; set; }
   }
}
