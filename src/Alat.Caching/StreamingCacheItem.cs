using System;
using System.IO;

namespace Alat.Caching {
   public class StreamingCacheItem : CacheItem {
      public string Key { get; set; }
      public string Tag { get; set; }
      public object Data { get; set; }
      public Stream Stream { get; set; }
      public DateTime ExpirationDate { get; set; }
   }
}
