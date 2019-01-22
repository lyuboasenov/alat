using System;
using Alat.Http.Caching;
using Alat.Http.Caching.Sessions;
using Alat.Testing.Mocks;

namespace Alat.Http.Tests.Mocks {
   internal class SessionMediatorMock : NotifyingMock, ISessionMediator {

      public ICache Cache { get; }
      public bool CachingEnabledByDefault { get; }
      public TimeSpan DefaultCachePeriod { get; }

      public SessionMediatorMock() {

      }

      public SessionMediatorMock(bool cachingEnabledByDefault, TimeSpan defaultCachePeriod) {
         CachingEnabledByDefault = cachingEnabledByDefault;
         DefaultCachePeriod = defaultCachePeriod;
      }

      public void AbbandonSession(Caching.Sessions.Session session) {
         MethodCalled(nameof(AbbandonSession), session);
      }

      public void OnKeyAdded(string key) {
         MethodCalled(nameof(OnKeyAdded), key);
      }

      public void CloseSession(Caching.Sessions.Session session) {
         MethodCalled(nameof(CloseSession), session);
      }

      public TimeSpan GetCachingPeriod() {
         MethodCalled(nameof(GetCachingPeriod));
         return DefaultCachePeriod;
      }

      public void RegisterSession(Caching.Sessions.Session session) {
         MethodCalled(nameof(RegisterSession), session);
      }

      public bool ShouldCache() {
         MethodCalled(nameof(ShouldCache));
         return CachingEnabledByDefault;
      }
   }
}
