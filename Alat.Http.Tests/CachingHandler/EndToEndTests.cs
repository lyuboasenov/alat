using Alat.Testing.Mocks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace Alat.Http.Tests.CachingHandler {
   public class EndToEndTests {
      [Fact]
      public void RegularCall() {
         var setup = Setup();

         setup.Client.GetAsync("http://dummy.com");
         Assert.False(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));

         using (var session = setup.Factory.OpenSession()) {
            setup.Client.GetAsync("http://dummy.com");
            Assert.True(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));
            session.Close();
         }

         Assert.False(setup.CacheObserver.IsMethodCalledIgnoreParameters("Remove"));
      }

      [Fact]
      public void AbbandonSession() {
         var setup = Setup();

         setup.Client.GetAsync("http://dummy.com");
         Assert.False(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));

         using (var session = setup.Factory.OpenSession()) {
            setup.Client.GetAsync("http://dummy.com");
            setup.Client.GetAsync("http://dummy1.com");
            setup.Client.GetAsync("http://dummy2.com");
            Assert.Equal(3, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
            session.Abbandon();
         }

         Assert.Contains(setup.CacheObserver.CalledMethods, 
            (cm) => cm.MethodName == "Remove" && ((IEnumerable<string>)cm.Parameters[0]).Count() == 3);
      }

      [Fact]
      public void CacheByDefault() {
         var setup = Setup(true);

         setup.Client.GetAsync("http://dummy.com");
         Assert.True(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));
      }

      [Fact]
      public void NestedSessions() {
         var setup = Setup();

         setup.Client.GetAsync("http://dummy.com");
         Assert.False(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));

         using (var session = setup.Factory.OpenSession()) {
            setup.Client.GetAsync("http://dummy.com");
            setup.Client.GetAsync("http://dummy1.com");
            setup.Client.GetAsync("http://dummy2.com");
            Assert.Equal(3, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
            using (var session2 = setup.Factory.OpenSession()) {
               setup.Client.GetAsync("http://dummy3.com");
               setup.Client.GetAsync("http://dummy4.com");
               Assert.Equal(5, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
               session2.Abbandon();
            }
            setup.Client.GetAsync("http://dummy5.com");
            Assert.Equal(6, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
            session.Close();
         }

         Assert.Equal(1, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Remove"));
         Assert.Contains(setup.CacheObserver.CalledMethods,
            (cm) => cm.MethodName == "Remove" && ((IEnumerable<string>)cm.Parameters[0]).Count() == 2);
      }


      [Fact]
      public void NestedSessionsAbandonOuter() {
         var setup = Setup();

         setup.Client.GetAsync("http://dummy.com");
         Assert.False(setup.CacheObserver.IsMethodCalledIgnoreParameters("Store"));

         var session = setup.Factory.OpenSession();

         setup.Client.GetAsync("http://dummy.com");
         setup.Client.GetAsync("http://dummy1.com");
         setup.Client.GetAsync("http://dummy2.com");
         Assert.Equal(3, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
         Caching.Sessions.Session session2 = setup.Factory.OpenSession();
         try {
            setup.Client.GetAsync("http://dummy3.com");
            setup.Client.GetAsync("http://dummy4.com");
            Assert.Equal(5, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
            session.Abbandon();
         } finally {
            Assert.Throws<ArgumentException>(() => session2.Dispose());
         }
         setup.Client.GetAsync("http://dummy5.com");
         Assert.Equal(5, setup.CacheObserver.CalledMethods.Count(cm => cm.MethodName == "Store"));
         Assert.Contains(setup.CacheObserver.CalledMethods,
            (cm) => cm.MethodName == "Remove" && ((IEnumerable<string>)cm.Parameters[0]).Count() == 2);
         Assert.Contains(setup.CacheObserver.CalledMethods,
            (cm) => cm.MethodName == "Remove" && ((IEnumerable<string>)cm.Parameters[0]).Count() == 3);
      }








      private SetupItems Setup(bool cacheByDefault = false) {
         var cache = new Mocks.NotifyingCacheMock();
         var cacheObserver = new NotifyingMockObserver();
         cache.Subscribe(cacheObserver);

         var factory = new Caching.Sessions.SessionFactory();
         var cacheHandler = new Caching.CachingHandler(factory,
            new Caching.Settings() {
               Cache = cache,
               CachePeriod = cacheByDefault ? TimeSpan.FromMinutes(3) : TimeSpan.FromMinutes(1),
               CachingEnabledByDefault = cacheByDefault
            }, new Mocks.ThrowingHttpHandler(int.MaxValue));

         return new SetupItems() {
            Client = new HttpClient(cacheHandler),
            Factory = cacheHandler.SessionFactory,
            CacheObserver = cacheObserver
         };
      }

      private class SetupItems {
         public HttpClient Client { get; set; }
         public Caching.Sessions.SessionFactory Factory { get; set; }
         public NotifyingMockObserver CacheObserver { get; set; }
      }
   }
}
