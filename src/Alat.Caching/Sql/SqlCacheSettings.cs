using Alat.Caching.Serialization;
using Alat.Caching.Services;

namespace Alat.Caching.Sql { 
   public sealed class SqlCacheSettings {
      public ISerializer Serializer { get; }
      public IFileSystemService FileSystem { get; }
      public ISqlService SqlService { get; }

      public SqlCacheSettings(ISqlService sqlService) : 
         this(sqlService, new XmlSerializerAdapter(), new FileSystemService()) { }

      public SqlCacheSettings(ISqlService sqlService, ISerializer serializer) : 
         this(sqlService, serializer, new FileSystemService()) { }

      public SqlCacheSettings(ISqlService sqlService, ISerializer serializer, IFileSystemService fileSystem) {
         SqlService = sqlService;
         Serializer = serializer;
         FileSystem = fileSystem;
      }
   }
}
