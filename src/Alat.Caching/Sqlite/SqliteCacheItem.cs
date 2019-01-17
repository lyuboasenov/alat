using Alat.Caching;
using SQLite;
using System;
using System.IO;

namespace Climbing.Guide.Caching.Sqlite {
   public class SqliteCacheItem : CacheItem {
      [PrimaryKey]
      public string Key { get; set; }
      public string Tag { get; set; }
      [Ignore]
      public Stream Content { get; set; }
      public DateTime ExpirationDate { get; set; }
      public byte[] RawContent { get; set; }
      public object Data { get; set; }
   }
}
