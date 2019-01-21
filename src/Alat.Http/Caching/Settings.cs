using System;

namespace Alat.Http.Caching {
   public sealed class Settings {
      public bool CachingEnabledByDefault { get; set; }
      public TimeSpan CachePeriod { get; set; }
      public ICache Cache { get; set; }
   }
}
