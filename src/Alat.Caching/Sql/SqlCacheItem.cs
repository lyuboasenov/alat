using System.IO;

namespace Alat.Caching.Sql {
   public class SqlCacheItem {
      public CacheItemMeta Meta { get; set; }
      public byte[] Data { get; set; }

      internal static SqlCacheItem FromStreamCacheItem(StreamCacheItem source) {
         using (MemoryStream memoryStream = new MemoryStream()) {
            source.Stream.CopyTo(memoryStream);
            var sqlCacheItem = new SqlCacheItem {
               Meta = source.Meta,
               Data = memoryStream.ToArray()
            };

            return sqlCacheItem;
         }
      }
   }
}
