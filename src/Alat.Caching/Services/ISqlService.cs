using Alat.Caching.Sql;
using System.Collections.Generic;

namespace Alat.Caching.Services {
   public interface ISqlService {
      void InitializeDatabase();

      void DeleteExpired();
      void Delete(IEnumerable<string> keys);
      void DeleteAll();

      bool Contains(string key);
      bool Any();

      void InsertOrUpdate(SqlCacheItem item);

      SqlCacheItem Find(string key);

      long GetSize();
   }
}
