using System;
using System.IO;
using System.Runtime.Serialization;

namespace Alat.Caching {
   public abstract class SerializingCacheStore : CacheStore {

      private IFormatter Formatter { get; }

      protected SerializingCacheStore(IFormatter formatter) {
         Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
      }

      public void Add<T>(string key, T data, string tag, DateTime expirationDate) {
         var cacheItem = new StreamingCacheItem() {
            Key = key,
            Tag = tag,
            ExpirationDate = expirationDate,
            Stream = Serialize(data)
         };

         Add(cacheItem);
      }

      public CacheItem Find<T>(string key) {
         StreamingCacheItem cacheItem = Find(key);
         cacheItem.Data = Deserialized<T>(cacheItem.Stream);

         return cacheItem;
      }

      public string FindTag(string key) {
         StreamingCacheItem cacheItem = Find(key);

         return null == cacheItem ? string.Empty : cacheItem.Tag;
      }

      public abstract bool Any();
      public abstract void Clean();
      public abstract bool Contains(string key);
      
      public abstract long GetSize();
      public abstract void Remove(string[] key);
      public abstract void RemoveAll();
      public abstract void Reset(string key, DateTime dateTime);

      protected abstract void Add(StreamingCacheItem cacheItem);
      protected abstract StreamingCacheItem Find(string key);

      private Stream Serialize<T>(T data) {
         Stream result = null;
         if (typeof(Stream).IsAssignableFrom(data.GetType())) {
            object boxed = data;
            result = (Stream)boxed;
         } else {
            result = new MemoryStream();
            Formatter.Serialize(result, data);
         }

         return result;
      }

      private object Deserialized<T>(Stream stream) {
         object result = stream;
         if (!typeof(Stream).IsAssignableFrom(typeof(T))) {
            return Formatter.Deserialize(stream);
         }

         return result;
      }
   }
}
