using Alat.Caching.Sql;
using Alat.Caching.Sqlite;
using System;
using System.IO;

namespace Alat.Caching.Tests.CacheStore.FileSystem {
   internal class FileSystemTestsContext : IDisposable {
      private readonly string Location = Guid.NewGuid().ToString("N");

      public FileSystemTestsContext() {
         Directory.CreateDirectory(Location);
      }

      public FileSystemCacheSettings GetCacheSettings() {
         return new FileSystemCacheSettings(Location);
      }

#pragma warning disable CA1063 // Implement IDisposable Correctly
      public void Dispose() {
#pragma warning restore CA1063 // Implement IDisposable Correctly
         Directory.Delete(Location, true);
      }
   }
}
