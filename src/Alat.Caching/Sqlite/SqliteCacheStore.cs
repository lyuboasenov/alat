using Alat.Caching;
using Alat.Caching.Services;
using SQLite;
using System;
using System.IO;

namespace Climbing.Guide.Caching.Sqlite {
   public class SqliteCacheStore : SerializingCacheStore {
      private readonly object dblock = new object();
      private SQLiteConnection DB { get; }
      private string DbFilePath { get; }
      private FileSystemService FileSystem { get; }

      public SqliteCacheStore(SqliteCacheSettings settings) : base(settings.Formatter) {
         FileSystem = settings.FileSystem;
         DbFilePath = Path.Combine(settings.Location, "climbing.guide.cache.db");
         FileSystem.EnsureDirectoryExists(settings.Location);

         DB = new SQLiteConnection(DbFilePath);
         DB.CreateTable<SqliteCacheItem>();

         Clean();
      }

      public override void Clean() {
         lock (dblock) {
            var entries = DB.Query<SqliteCacheItem>($"SELECT * FROM SqliteCacheItem WHERE ExpirationDate < ?", DateTime.UtcNow.Ticks);
            DB.RunInTransaction(() => {
               foreach (var k in entries)
                  DB.Delete<SqliteCacheItem>(k.Key);
            });
         }
      }

      public override bool Contains(string key) {
         return DB.Find<SqliteCacheItem>(key) != null;
      }

      public override bool Any() {
         lock (dblock) {
            return DB.Table<SqliteCacheItem>().Count() > 0;
         }
      }

      public override void Reset(string key, DateTime expireAt) {
         var item = Find(key);
         item.ExpirationDate = expireAt;
         lock (dblock) {
            DB.Update(item);
         }
      }

      public override long GetSize() {
         return FileSystem.GetFileSize(DbFilePath);
      }

      public override void Remove(string[] keys) {
         lock (dblock) {
            foreach(var key in keys) {
               DB.Delete<SqliteCacheItem>(key);
            }
         }
      }

      public override void RemoveAll() {
         lock (dblock) {
            DB.DeleteAll<SqliteCacheItem>();
         }
      }

      protected override void Add(StreamingCacheItem cacheItem) {
         using (MemoryStream memoryStream = new MemoryStream()) {
            cacheItem.Stream.CopyTo(memoryStream);
            var ent = new SqliteCacheItem {
               Key = cacheItem.Key,
               ExpirationDate = cacheItem.ExpirationDate,
               Tag = cacheItem.Tag,
               RawContent = memoryStream.ToArray()
            };

            lock (dblock) {
               DB.InsertOrReplace(ent);
            }
         }
      }

      protected override StreamingCacheItem Find(string key) {
         SqliteCacheItem item;
         StreamingCacheItem result = null;
         lock (dblock) {
            item = DB.Find<SqliteCacheItem>(key);
         }
         if (null != item) {
            var stream = new MemoryStream(item.RawContent);
            stream.Seek(0, SeekOrigin.Begin);

            result = new StreamingCacheItem() {
               Key = item.Key,
               Tag = item.Tag,
               ExpirationDate = item.ExpirationDate,
               Stream = stream
            };
         }

         return result;
      }
   }
}
