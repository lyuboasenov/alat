using Alat.Caching.Serialization;
using Alat.Caching.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Alat.Caching.FileSystem {
   internal class FileSystemCacheStore : SerializingCacheStore {
      private const string ITEMS_DIRECTORY = "items";
      private readonly object lockObj = new object();

      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();

      private IDictionary<string, CacheItemMeta> Items { get; }

      private string IndexFilePath { get; }
      private XmlSerializer IndexSerializer { get; }

      private string Location { get; }
      private IFileSystemService FileSystem { get; }

      public FileSystemCacheStore(FileSystemCacheSettings settings) : base(settings.Serializer) {
         Location = settings.Location;
         FileSystem = settings.FileSystem;

         IndexSerializer = new XmlSerializer(typeof(CacheItemMeta[]));
         Items = new Dictionary<string, CacheItemMeta>();
         IndexFilePath = Path.Combine(Location, "climbing.guide.cache.index");
         FileSystem.EnsureDirectoryExists(Location);
      }

      public override void RemoveExpired() {
         lock (lockObj) {
            var keysToRemove = Items.
               Where(pair => pair.Value.ExpirationDate < DateTime.UtcNow).
               Select(p => p.Key).ToArray();

            foreach (var key in keysToRemove) {
               var item = Items[key];
               Items.Remove(key);
            }
            SaveIndex();
         }
      }

      public override bool Contains(string key) {
         KeyValidator.ValidateKey(key);

         return Items.ContainsKey(key);
      }

      public override bool Any() {
         return Items.Count > 0;
      }

      public override void Remove(string key) {
         Remove(new[] { key });
      }

      public override void Remove(IEnumerable<string> keys) {
         KeyValidator.ValidateKeys(keys);

         lock (lockObj) {
            foreach(var key in keys) {
               if (Contains(key)) {
                  var item = Items[key];
                  FileSystem.DeleteFile(GetFileName(key));
                  Items.Remove(key);
               }
            }
            SaveIndex();
         }
      }

      public override void RemoveAll() {
         lock (lockObj) {
            Items.Clear();
            SaveIndex();
         }

         FileSystem.DeleteDirectory(Path.Combine(Location, ITEMS_DIRECTORY));
         FileSystem.EnsureDirectoryExists(Path.Combine(Location, ITEMS_DIRECTORY));
      }

      public override long GetSize() {
         return FileSystem.GetDirectorySize(Location);
      }

      protected override void StoreStreamCacheItem(StreamCacheItem cacheItem) {
         KeyValidator.ValidateKey(cacheItem.Meta.Key);

         lock (lockObj) {
            FileSystem.EnsureDirectoryExists(Path.Combine(Location, ITEMS_DIRECTORY));
            using (var file =
               FileSystem.GetFileWriteStream(GetFileName(cacheItem.Meta.Key))) {
               cacheItem.Stream.CopyTo(file);
            }

            Items[cacheItem.Meta.Key] = cacheItem.Meta;
            SaveIndex();
         }
      }

      protected override StreamCacheItem RetrieveStreamCacheItem(string key) {
         KeyValidator.ValidateKey(key);

         StreamCacheItem streamingCacheItem = null;
         if (Contains(key)) {
            streamingCacheItem =
               StreamCacheItem.FromCacheItem(Items[key], FileSystem.GetFileReadStream(GetFileName(key)), true);
         }

         return streamingCacheItem;
      }

      private void LoadIndex() {
         using (var indexFile = FileSystem.GetFileReadStream(IndexFilePath)) {
            Items.Clear();
            var loadedItems = (CacheItemMeta[])IndexSerializer.Deserialize(indexFile);
            foreach(var item in loadedItems) {
               Items.Add(item.Key, item);
            }
         }
      }

      private void SaveIndex() {
         using (var indexFile = FileSystem.GetFileWriteStream(IndexFilePath)) {
            IndexSerializer.Serialize(indexFile, Items.Values.ToArray());
         }
      }

      private string GetFileName(string key) {
         KeyValidator.ValidateKey(key);

         return Path.Combine(Location, ITEMS_DIRECTORY, key.GetHashCode().ToString());
      }
   }
}
