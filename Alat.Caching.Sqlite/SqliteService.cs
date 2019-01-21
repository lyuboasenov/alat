using Alat.Caching.Services;
using Alat.Caching.Sql;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace Alat.Caching.Sqlite {
   public class SqliteService : ISqlService {
      private readonly object dblock = new object();

      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();
      private string Location { get; }
      private string DbFilePath { get; }
      private SQLiteConnection DB { get; set; }

      public SqliteService(string baseLocation) {
         Location = baseLocation;
         DbFilePath = Path.Combine(Location, "alat.caching.sqlite.db");
      }

      public void InitializeDatabase() {
         lock (dblock) {
            DB = new SQLiteConnection(DbFilePath);
            DB.CreateTable<SqliteCacheItem>();
         }
      }

      public void DeleteExpired() {
         var entities = DB.Query<SqliteCacheItem>($"SELECT * FROM SqliteCacheItem WHERE ExpirationDate < ?", DateTime.UtcNow.Ticks);
         lock (dblock) {
            DB.RunInTransaction(() => {
               foreach (var k in entities)
                  DB.Delete<SqliteCacheItem>(k.Key);
            });
         }
      }

      public void Delete(IEnumerable<string> keys) {
         KeyValidator.ValidateKeys(keys);

         lock (dblock) {
            foreach (var key in keys) {
               DB.Delete<SqliteCacheItem>(key);
            }
         }
      }

      public void DeleteAll() {
         lock (dblock) {
            DB.DeleteAll<SqliteCacheItem>();
         }
      }

      public bool Contains(string key) {
         KeyValidator.ValidateKey(key);

         return DB.Find<SqliteCacheItem>(key) != null;
      }

      public bool Any() {
         return DB.Table<SqliteCacheItem>().Count() > 0;
      }

      public long GetSize() {
         var fileInfo = new FileInfo(DbFilePath);
         return fileInfo.Length;
      }

      public void InsertOrUpdate(SqlCacheItem item) {
         if (null == item) {
            throw new ArgumentNullException(nameof(item));
         }

         KeyValidator.ValidateKey(item.Meta.Key);

         var sqliteItem = SqliteCacheItem.FromSqlCacheItem(item);

         lock (dblock) {
            DB.InsertOrReplace(sqliteItem);
         }
      }

      public SqlCacheItem Find(string key) {
         KeyValidator.ValidateKey(key);

         lock (dblock) {
            return DB.Find<SqliteCacheItem>(key)?.ToSqlCacheItem();
         }
      }

      private static void EnsureDirectoryExists(string path) {
         if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
         }
      }
   }
}
