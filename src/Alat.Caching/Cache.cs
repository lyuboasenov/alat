using System;
using System.Collections.Generic;
using System.Threading;

namespace Alat.Caching {
   public class Cache : ICache {
      private const int DEFAULT_CLEANUP_TIME_MINUTES = 5;

      private KeyValidator KeyValidator { get; } = KeyValidator.GetValidator();
      private TimeSpan CleanInterval { get; }
      private ICacheStore CacheStore { get; }
      private Timer CleanupTimer { get; }

      public Cache(ICacheStore cacheStore) : 
         this(cacheStore, TimeSpan.FromMinutes(DEFAULT_CLEANUP_TIME_MINUTES)) { }

      public Cache(ICacheStore cacheStore, TimeSpan cleanInterval) {

         CacheStore = cacheStore ?? throw new ArgumentNullException(nameof(cacheStore));

         CleanInterval = cleanInterval;
         CleanupTimer = new Timer((state) => Clean());
         ScheduleNextClean();
      }

      public bool Contains(string key) {
         KeyValidator.ValidateKey(key);
         return CacheStore.Contains(key);
      }

      public T Retrieve<T>(string key) {
         KeyValidator.ValidateKey(key);

         var cacheItem = CacheStore.Retrieve<T>(key);
         return null == cacheItem ? default(T) : (T)cacheItem.Data;
      }

#pragma warning disable CA1822 // Mark members as static
      public T Retrieve<T>(string key, string tag) {
#pragma warning restore CA1822 // Mark members as static
         throw new NotImplementedException();
      }

      public void Store<TData>(string key, TData data) {
         Store(key, data, TimeSpan.MaxValue);
      }

      public void Store<TData>(string key, TData data, string tag = null) {
         Store(key, data, TimeSpan.MaxValue, tag);
      }

      public void Store<TData>(string key, TData data, TimeSpan expireIn) {
         Store(key, data, expireIn, null);
      }

      public void Store<TData>(string key, TData data, TimeSpan expireIn, string tag = null) {
         KeyValidator.ValidateKey(key);

         if (null == data)
            throw new ArgumentNullException(nameof(data));

         var item = new CacheItem<TData>() {
            Meta = new CacheItemMeta() {
               Key = key,
               Tag = tag,
               ExpirationDate = GetExpiration(expireIn)
            },
            Data = data
         };

         CacheStore.Store(item);
         ScheduleNextClean();
      }

      public void Remove(string key) {
         KeyValidator.ValidateKey(key);

         CacheStore.Remove(new[] { key });
      }

#pragma warning disable CA1822 // Mark members as static
      public void Remove(string key, string tag) {
#pragma warning restore CA1822 // Mark members as static
         throw new NotImplementedException();
      }

      public void Remove(IEnumerable<string> keys) {
         KeyValidator.ValidateKeys(keys);

         CacheStore.Remove(keys);
      }

      public void RemoveAll() {
         CacheStore.RemoveAll();
      }

      public void RemoveExired() {
         CacheStore.RemoveExpired();
      }

      public void Clean() {
         CacheStore.RemoveExpired();
         ScheduleNextClean();
      }

      public long GetCacheSize() {
         return CacheStore.GetSize();
      }

      private static bool IsString<T>(T data) {
         var typeOf = typeof(T);
         if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>)) {
            typeOf = Nullable.GetUnderlyingType(typeOf);
         }
         var typeCode = Type.GetTypeCode(typeOf);
         return typeCode == TypeCode.String;
      }

      private static DateTime GetExpiration(TimeSpan expiration) {
         
         if (expiration < expiration.Duration() || expiration == TimeSpan.Zero) {
            throw new ArgumentException($"{nameof(expiration)} should be positive time span");
         }

         var result = DateTime.MaxValue;
         if (expiration != TimeSpan.MaxValue) {
            result = DateTime.UtcNow.Add(expiration);
         }
         return result;
      }

      private void ScheduleNextClean() {
         if (CacheStore.Any()) {
            int dueTime = CleanInterval.Milliseconds == 0 ? Timeout.Infinite : CleanInterval.Milliseconds;
            CleanupTimer.Change(dueTime, Timeout.Infinite);
         }
      }
   }
}
