using Alat.Caching.Serialization;
using Alat.Caching.Services;

namespace Alat.Caching { 
   public sealed class FileSystemCacheSettings {
      public string Location { get; }
      public ISerializer Serializer { get; }
      public IFileSystemService FileSystem { get; }

      public FileSystemCacheSettings(string location) : 
         this(location, new XmlSerializerAdapter(), new FileSystemService()) { }

      public FileSystemCacheSettings(string location, ISerializer serializer) : 
         this(location, serializer, new FileSystemService()) { }

      public FileSystemCacheSettings(string location, ISerializer serializer, IFileSystemService fileSystem) {
         Location = location;
         Serializer = serializer;
         FileSystem = fileSystem;
      }
   }
}
