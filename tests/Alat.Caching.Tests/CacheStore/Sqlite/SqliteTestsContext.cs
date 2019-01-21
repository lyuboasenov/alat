using Alat.Caching.Sql;
using Alat.Caching.Sqlite;
using System;
using System.IO;

namespace Alat.Caching.Tests.CacheStore.Sqlite {
   internal class SqliteTestsContext : IDisposable {
      private readonly string Location = Guid.NewGuid().ToString("N");

      public SqliteTestsContext() {
         Directory.CreateDirectory(Location);
      }

      public SqlCacheSettings GetCacheSettings() {
         var sqlService = new SqliteService(Location);
         return new SqlCacheSettings(sqlService);
      }

#pragma warning disable CA1063 // Implement IDisposable Correctly
      public void Dispose() {
#pragma warning restore CA1063 // Implement IDisposable Correctly
         Directory.Delete(Location, true);
      }
   }
}
