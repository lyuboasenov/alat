namespace Alat.Caching.FileSystem {
   public class FileSystemCache : Impl.Cache {
      public FileSystemCache(FileSystemCacheSettings settings) :
         base(new FileSystemCacheStore(settings)) { }
   }
}
