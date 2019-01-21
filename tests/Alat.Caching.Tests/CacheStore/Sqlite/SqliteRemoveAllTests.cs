using Alat.Caching.Sql;

namespace Alat.Caching.Tests.CacheStore.Sqlite {
   public class SqliteRemoveAllTests : RemoveAllTests {
      SqliteTestsContext SqliteTestsContext { get; } = new SqliteTestsContext();

      protected override Caching.ICacheStore CreateCacheStore() {
         return new SqlCacheStore(SqliteTestsContext.GetCacheSettings());
      }
   }
}
