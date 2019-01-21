using Alat.Caching.Sql;

namespace Alat.Caching.Tests.CacheStore.Sqlite {
   public class SqliteRemoveTests : RemoveTests {
      SqliteTestsContext SqliteTestsContext { get; } = new SqliteTestsContext();

      protected override Caching.ICacheStore CreateCacheStore() {
         return new SqlCacheStore(SqliteTestsContext.GetCacheSettings());
      }
   }
}
