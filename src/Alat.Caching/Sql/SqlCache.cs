namespace Alat.Caching.Sql {
   public class SqlCache : Cache {
      public SqlCache(SqlCacheSettings settings) : 
         base(new SqlCacheStore(settings)) { }
   }
}
