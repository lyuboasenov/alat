using System;
using System.Collections.Generic;

namespace Alat.Http.Caching.Sessions {
   internal class SessionFrame {
      public Guid SessionId { get; }
      public IList<string> Keys { get; }
      public TimeSpan CachingPeriod { get; }

      public SessionFrame(Guid id, TimeSpan cachingPeriod) {
         SessionId = id;
         CachingPeriod = cachingPeriod;
         Keys = new List<string>();
      }
   }
}
