using Alat.Caching;

namespace Climbing.Guide.Caching.Sqlite {
   public class SqliteCache : Alat.Caching.Impl.Cache {
      public SqliteCache(SqliteCacheSettings settings) : 
         base(new SqliteCacheStore(settings)) { }
   }
}
