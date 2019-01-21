namespace Alat.Caching.Memory {
   public class ReferencingMemoryCache : Cache {
      public ReferencingMemoryCache() :
         base(new ReferencingMemoryCacheStore()) { }
   }
}
