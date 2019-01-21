using System;

namespace Alat.Caching {
   public class CacheItemMeta {
      public string Key { get; set; }
      public string Tag { get; set; }
      public DateTime ExpirationDate { get; set; }
   }
}
