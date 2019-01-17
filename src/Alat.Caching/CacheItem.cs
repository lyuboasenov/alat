using System;

namespace Alat.Caching {
   public interface CacheItem {
      string Key { get; }

      /// <summary>
      /// Additional Tags
      /// </summary>
      string Tag { get; }

      /// <summary>
      /// Main Content.
      /// </summary>
      object Data { get; }

      /// <summary>
      /// Expiration data of the object, stored in UTC
      /// </summary>
      DateTime ExpirationDate { get; }
   }
}
