namespace Alat.Caching.FileSystem {
   public class FileSystemCache : Cache {
      public FileSystemCache(FileSystemCacheSettings settings) :
         base(new FileSystemCacheStore(settings)) { }
   }
}
