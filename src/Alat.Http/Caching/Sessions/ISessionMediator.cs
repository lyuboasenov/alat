using System;

namespace Alat.Http.Caching.Sessions {
   internal interface ISessionMediator {
      ICache Cache { get; }
      bool CachingEnabledByDefault { get; }
      TimeSpan DefaultCachePeriod { get; }

      void AbbandonSession(Session session);
      void OnKeyAdded(string key);
      void CloseSession(Session session);
      TimeSpan GetCachingPeriod();
      void RegisterSession(Session session);
      bool ShouldCache();
   }
}