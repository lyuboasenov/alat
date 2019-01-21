using System;
using Xunit;

namespace Alat.Http.Tests.CachingHandler {
   public class CreateInstanceTests {
      private readonly Mocks.CacheMock Cache = new Mocks.CacheMock();

      [Fact]
      public void SettingsOnly() {
         var handler = CreateCachingHandler(Cache, TimeSpan.FromDays(1), true);

         Assert.NotNull(handler);
         Assert.NotNull(handler.SessionFactory);
         Assert.NotNull(handler.InnerHandler);
      }

      [Fact]
      public void SettingsAndFactory() {
         var sessionFactory = new Caching.Sessions.SessionFactory();
         var handler = CreateCachingHandler(Cache, TimeSpan.FromDays(1), true, sessionFactory);

         Assert.NotNull(handler);
         Assert.NotNull(handler.SessionFactory);
         Assert.NotNull(handler.InnerHandler);
         Assert.Equal(sessionFactory, handler.SessionFactory);
      }

      [Fact]
      public void NullSettingsAndFactory() {
         var sessionFactory = new Caching.Sessions.SessionFactory();
         Assert.Throws<ArgumentNullException>(()=> new Caching.CachingHandler(sessionFactory, null));
      }

      [Fact]
      public void SettingsAndNullFactory() {
         Assert.Throws<ArgumentNullException>(()=> CreateCachingHandler(Cache, TimeSpan.FromDays(1), true, null));
      }


      [Fact]
      public void NullCache() {
         Assert.Throws<ArgumentNullException>(()=> CreateCachingHandler(null, TimeSpan.FromDays(1), true));
      }

      [Fact]
      public void NegativePeriod() {
         Assert.Throws<ArgumentException>(()=> CreateCachingHandler(Cache, TimeSpan.FromDays(-1), true));
      }

      private static Caching.CachingHandler CreateCachingHandler(
         Caching.ICache cache, 
         TimeSpan period, 
         bool cachingEnabled, 
         Caching.Sessions.SessionFactory sessionFactory) {
         return new Caching.CachingHandler(sessionFactory,
            new Caching.Settings() {
               Cache = cache,
               CachePeriod = TimeSpan.FromMinutes(5),
               CachingEnabledByDefault = false
            });
      }

      private static Caching.CachingHandler CreateCachingHandler(
         Caching.ICache cache, 
         TimeSpan period, 
         bool cachingEnabled) {
         return new Caching.CachingHandler(new Caching.Settings() {
            Cache = cache,
            CachePeriod = period,
            CachingEnabledByDefault = false
         });
      }
   }
}
