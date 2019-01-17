using Alat.Caching.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alat.Caching.FileSystem {
   public class FileSystemCacheStore : SerializingCacheStore {
      private const string ITEMS_DIRECTORY = "items";
      private readonly object lockObj = new object();
      private string IndexFilePath { get; }
      private string Location { get; }
      private IDictionary<string, StreamingCacheItem> Items { get; } = new Dictionary<string, StreamingCacheItem>();
      private FileSystemService FileSystem { get; }

      public FileSystemCacheStore(FileSystemCacheSettings settings) : base(settings.Formatter) {
         Location = settings.Location;
         FileSystem = settings.FileSystem;

         IndexFilePath = Path.Combine(Location, "climbing.guide.cache.index");
         FileSystem.EnsureDirectoryExists(Location);
      }

      public override void Clean() {
         lock (lockObj) {
            var keysToRemove = Items.
               Where(pair => pair.Value.ExpirationDate < DateTime.Now).
               Select(p => p.Key).ToArray();

            foreach (var key in keysToRemove) {
               var item = Items[key];
               Items.Remove(key);
            }
            SaveIndex();
         }
      }

      public override bool Contains(string key) {
         return Items.ContainsKey(key);
      }

      public override bool Any() {
         return Items.Count > 0;
      }

      public override void Reset(string key, DateTime dateTime) {
         if (Contains(key)) {
            lock (lockObj) {
               Items[key].ExpirationDate = dateTime;
               SaveIndex();
            }
         }
      }

      public override void Remove(string[] keys) {
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
      }

      public override long GetSize() {
         return FileSystem.GetDirectorySize(Location);
      }

      protected override void Add(StreamingCacheItem cacheItem) {
         lock (lockObj) {
            FileSystem.EnsureDirectoryExists(Path.Combine(Location, ITEMS_DIRECTORY));
            using (var file =
               FileSystem.GetFileCreateStream(GetFileName(cacheItem.Key))) {
               cacheItem.Stream.CopyTo(file);
            }

            Items[cacheItem.Key] = cacheItem;
            SaveIndex();
         }
      }

      protected override StreamingCacheItem Find(string key) {
         StreamingCacheItem streamingCacheItem = null;
         if (Contains(key)) {
            var item = Items[key];
            streamingCacheItem = new StreamingCacheItem() {
               Key = item.Key,
               Tag = item.Tag,
               ExpirationDate = item.ExpirationDate,
               Stream = FileSystem.GetFileReadStream(GetFileName(key))
            };
         }

         return streamingCacheItem;
      }

      private void LoadIndex() {
         System.Xml.Serialization.XmlSerializer reader =
            new System.Xml.Serialization.XmlSerializer(typeof(Dictionary<string, StreamingCacheItem>));

         using (var indexFile = FileSystem.GetFileReadStream(IndexFilePath)) {
            Items.Clear();
            var loadedItems = (Dictionary<string, StreamingCacheItem>)reader.Deserialize(indexFile);
            foreach(var item in loadedItems) {
               Items.Add(item);
            }
         }
      }

      private void SaveIndex() {
         System.Xml.Serialization.XmlSerializer writer =
            new System.Xml.Serialization.XmlSerializer(typeof(Dictionary<string, StreamingCacheItem>));

         using (var indexFile = FileSystem.GetFileWriteStream(IndexFilePath)) {
            writer.Serialize(indexFile, Items);
         }
      }

      private string GetFileName(string key) {
         return Path.Combine(Location, ITEMS_DIRECTORY, key.GetHashCode().ToString());
      }
   }
}
