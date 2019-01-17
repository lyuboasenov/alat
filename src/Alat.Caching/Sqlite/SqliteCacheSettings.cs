using Alat.Caching.Services;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Alat.Caching { 
   public sealed class SqliteCacheSettings {
      public string Location { get; }
      public IFormatter Formatter { get; }
      public FileSystemService FileSystem { get; }

      public SqliteCacheSettings(string location) : 
         this(location, new BinaryFormatter(), new Services.Impl.FileSystemService()) { }

      public SqliteCacheSettings(string location, IFormatter formatter) : 
         this(location, formatter, new Services.Impl.FileSystemService()) { }

      public SqliteCacheSettings(string location, IFormatter formatter, FileSystemService fileSystem) {
         Location = location;
         Formatter = formatter;
         FileSystem = fileSystem;
      }
   }
}
