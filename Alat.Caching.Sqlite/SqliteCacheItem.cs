using Alat.Caching.Sql;
using SQLite;
using System;

namespace Alat.Caching.Sqlite {
   public sealed class SqliteCacheItem {
      [PrimaryKey]
      public string Key { get; set; }

      public string Tag { get; set; }

      public DateTime ExpirationDate { get; set; }

#pragma warning disable CA1819 // Properties should not return arrays
      public byte[] Data { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

      internal static SqliteCacheItem FromSqlCacheItem(SqlCacheItem source) {
         return new SqliteCacheItem() {
            Key = source.Meta.Key,
            Tag = source.Meta.Tag,
            Data = source.Data,
            ExpirationDate = source.Meta.ExpirationDate
         };
      }

      internal SqlCacheItem ToSqlCacheItem() {
         return new SqlCacheItem() {
            Meta = new CacheItemMeta() {
               Key = Key,
               Tag = Tag,
               ExpirationDate = ExpirationDate
            },
            Data = Data            
         };
      }
   }
}
