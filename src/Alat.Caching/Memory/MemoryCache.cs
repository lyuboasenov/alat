namespace Alat.Caching.Memory {
   public class MemoryCache : Impl.Cache {
      public MemoryCache() :
         base(new MemoryCacheStore()) { }
   }
}
