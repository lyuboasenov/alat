﻿using Alat.Caching.FileSystem;

namespace Alat.Caching.Tests.CacheStore.FileSystem {
   public class FileSystemRemoveTests : RemoveTests {
      FileSystemTestsContext FileSystemTestsContext { get; } = new FileSystemTestsContext();

      protected override ICacheStore CreateCacheStore() {
         return new FileSystemCacheStore(FileSystemTestsContext.GetCacheSettings());
      }
   }
}
