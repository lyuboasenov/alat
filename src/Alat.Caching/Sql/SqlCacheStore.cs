using Alat.Caching.Serialization;
using Alat.Caching.Services;
using System.Collections.Generic;

namespace Alat.Caching.Sql {
   internal class SqlCacheStore : SerializingCacheStore {
      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();
      private IFileSystemService FileSystem { get; }
      private ISqlService SqlService { get; }

      public SqlCacheStore(SqlCacheSettings settings) : base(settings.Serializer) {
         SqlService = settings.SqlService;
         FileSystem = settings.FileSystem;

         SqlService.InitializeDatabase();

         RemoveExpired();
      }

      public override bool Contains(string key) {
         KeyValidator.ValidateKey(key);

         return SqlService.Contains(key);
      }

      public override bool Any() {
         return SqlService.Any();
      }

      public override long GetSize() {
         return SqlService.GetSize();
      }

      public override void Remove(string key) {
         KeyValidator.ValidateKey(key);

         SqlService.Delete(new[] { key });
      }

      public override void Remove(IEnumerable<string> keys) {
         KeyValidator.ValidateKeys(keys);

         SqlService.Delete(keys);
      }

      public override void RemoveAll() {
         SqlService.DeleteAll();
      }

      public override void RemoveExpired() {
         SqlService.DeleteExpired();
      }

      protected override void StoreStreamCacheItem(StreamCacheItem cacheItem) {
         KeyValidator.ValidateKey(cacheItem.Meta.Key);

         SqlService.InsertOrUpdate(SqlCacheItem.FromStreamCacheItem(cacheItem));
      }

      protected override StreamCacheItem RetrieveStreamCacheItem(string key) {
         KeyValidator.ValidateKey(key);

         return ConvertToStreamItem(SqlService.Find(key));
      }

      private static StreamCacheItem ConvertToStreamItem(SqlCacheItem sqlCacheItem) {
         StreamCacheItem result = default(StreamCacheItem);
         if (null != sqlCacheItem) {
            var stream = new System.IO.MemoryStream(sqlCacheItem.Data);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            result = StreamCacheItem.FromCacheItem(sqlCacheItem.Meta, stream, true);
         }

         return result;
      }
   }
}
