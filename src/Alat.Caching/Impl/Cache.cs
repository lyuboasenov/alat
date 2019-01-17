using System;
using System.Linq;
using System.Threading;

namespace Alat.Caching.Impl {
   public class Cache : Caching.Cache {
      private const int DEFAULT_CLEANUP_TIME_MINUTES = 5;

      private TimeSpan CleanInterval { get; }
      private CacheStore CacheStore { get; }
      private Timer CleanupTimer { get; }
      

      public Cache(CacheStore cacheStore) : 
         this(cacheStore, TimeSpan.FromMinutes(DEFAULT_CLEANUP_TIME_MINUTES)) { }

      public Cache(CacheStore cacheStore, TimeSpan cleanInterval) {

         CacheStore = cacheStore ?? throw new ArgumentNullException(nameof(cacheStore));

         CleanInterval = cleanInterval;
         CleanupTimer = new Timer((state) => Clean());
         StartAutoClean();
      }

      public bool Contains(string key) {
         ValidateKey(key);
         return CacheStore.Contains(key);
      }

      public T FindData<T>(string key) {
         ValidateKey(key);

         var cacheItem = CacheStore.Find<T>(key);
         return null == cacheItem ? default(T) : (T)cacheItem.Data;
      }

      public string FindTag(string key) {
         ValidateKey(key);

         return CacheStore.FindTag(key);
      }

      public void Add<T>(string key, T data, TimeSpan expireIn, string tag = null) {
         ValidateKey(key);

         if (null == data)
            throw new ArgumentNullException(nameof(data));

         CacheStore.Add(key, data, tag, GetExpiration(expireIn));
         StartAutoClean();
      }

      public void Remove(params string[] keys) {
         ValidateKey(keys);
         CacheStore.Remove(keys);
      }

      public void Reset(string key, TimeSpan expireIn) {
         ValidateKey(key);
         CacheStore.Reset(key, GetExpiration(expireIn));
      }

      public void Clean() {
         CacheStore.Clean();
         StartAutoClean();
      }

      public void Invalidate() {
         CacheStore.RemoveAll();
      }

      public long GetCacheSize() {
         return CacheStore.GetSize();
      }

      private static void ValidateKey(string key) {
         if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentNullException(nameof(key));
         }
      }

      private static void ValidateKey(params string[] keys) {
         if (null == keys || !keys.Any()) {
            throw new ArgumentNullException(nameof(keys));
         }

         foreach(var key in keys) {
            ValidateKey(key);
         }
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

      private void StartAutoClean() {
         if (CacheStore.Any()) {
            int dueTime = CleanInterval.Milliseconds == 0 ? Timeout.Infinite : CleanInterval.Milliseconds;
            CleanupTimer.Change(dueTime, Timeout.Infinite);
         }
      }
   }
}
