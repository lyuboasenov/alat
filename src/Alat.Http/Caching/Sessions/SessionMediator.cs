using System;

namespace Alat.Http.Caching.Sessions {
   internal sealed class SessionMediator : ISessionMediator {
      public TimeSpan DefaultCachePeriod { get; private set; }
      public bool CachingEnabledByDefault { get; private set; }
      public ICache Cache { get; private set; }

      private SessionStack SessionStack { get; }

      internal SessionMediator(ICache cache, bool cachingEnabledByDefault, TimeSpan defaultCachePeriod) {
         SessionStack = new SessionStack();

         Update(cache, cachingEnabledByDefault, defaultCachePeriod);
      }

      internal void Update(ICache cache, bool cachingEnabledByDefault, TimeSpan defaultCachePeriod) {
         Cache = cache ?? throw new ArgumentNullException(nameof(cache));

         if (cachingEnabledByDefault && defaultCachePeriod <= TimeSpan.Zero) {
            throw new ArgumentException(nameof(defaultCachePeriod));
         }

         CachingEnabledByDefault = cachingEnabledByDefault;
         DefaultCachePeriod = defaultCachePeriod;
      }

      public void OnKeyAdded(string key) {
         SessionStack.AddKeyToTopFrame(key);
      }

      public bool ShouldCache() {
         return CachingEnabledByDefault || !SessionStack.IsEmpty();
      }

      public TimeSpan GetCachingPeriod() {
         if (!ShouldCache()) {
            throw new InvalidOperationException("Handler not caching");
         }

         return SessionStack.IsEmpty() ? DefaultCachePeriod : SessionStack.GetCachingPeriod();
      }

      public void RegisterSession(Session session) {
         SessionStack.Push(new SessionFrame(session.Guid, session.CachingPeriod));
      }

      public void CloseSession(Session session) {
         SessionStack.Pop(session.Guid);
      }

      public void AbbandonSession(Session session) {
         var frame = SessionStack.Pop(session.Guid);
         Cache.Remove(frame.Keys);
      }
   }
}
