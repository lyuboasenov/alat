namespace Alat.Caching {
   public class CacheItem<TData> {
      public CacheItemMeta Meta { get; set; }
      public TData Data { get; set; }
   }
}
