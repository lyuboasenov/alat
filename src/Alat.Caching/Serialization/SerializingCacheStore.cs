using System;
using System.Collections.Generic;
using System.IO;

namespace Alat.Caching.Serialization {
   public abstract class SerializingCacheStore : ICacheStore {

      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();
      private ISerializer Serializer { get; }

      protected SerializingCacheStore(ISerializer serializer) {
         Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
      }

      public void Store<TData>(CacheItem<TData> item) {
         KeyValidator.ValidateKey(item.Meta.Key);

         if (null == item.Data) {
            throw new ArgumentNullException(nameof(item.Data));
         }

         StoreStreamCacheItem(
            StreamCacheItem.FromCacheItem(item.Meta, ConvertToStream(item.Data), IsTDataStream<TData>()));
      }

      public CacheItem<TData> Retrieve<TData>(string key) {
         KeyValidator.ValidateKey(key);

         var streamItem = RetrieveStreamCacheItem(key);

         return ConvertToCacheItem<TData>(streamItem);
      }

      public abstract bool Any();
      public abstract bool Contains(string key);

      public abstract void Remove(string key);
      public abstract void Remove(IEnumerable<string> keys);
      public abstract void RemoveAll();
      public abstract void RemoveExpired();
            
      public abstract long GetSize();

      protected abstract void StoreStreamCacheItem(StreamCacheItem cacheItem);
      protected abstract StreamCacheItem RetrieveStreamCacheItem(string key);

      private Stream ConvertToStream<TData>(TData data) {
         Stream result;
         if (IsTDataStream<TData>()) {
            object boxedStream = data;
            result = (Stream)boxedStream;
         } else {
            result = new MemoryStream();
            Serializer.Serialize(result, data);
            result.Seek(0, SeekOrigin.Begin);
         }

         return result;
      }

      private CacheItem<TData> ConvertToCacheItem<TData>(StreamCacheItem streamItem) {
         CacheItem<TData> result = default(CacheItem<TData>);
         if(null != streamItem) {
            result = new CacheItem<TData>() {
               Meta = streamItem.Meta,
               Data = ConvertFromStream<TData>(streamItem.Stream)
            };
         }

         return result;
      }

      private TData ConvertFromStream<TData>(Stream stream) {
         TData result;
         if (IsTDataStream<TData>()) {
            var resultStream = new MemoryStream();
            stream.CopyTo(resultStream);
            object boxedResult = resultStream;

            result = (TData)boxedResult;
         } else {
            result = Serializer.Deserialize<TData>(stream);
         }

         return result;
      }

      private static bool IsTDataStream<TData>() {
         return typeof(Stream).IsAssignableFrom(typeof(TData));
      }
   }
}
