using System;
using Xunit;

namespace Alat.Http.Tests.SessionFactory {
   public class SessionFactoryTests {
      [Fact]
      public void CreateInstanceNoParameters() {
         Assert.NotNull(new Caching.Sessions.SessionFactory());
      }

      [Fact]
      public void OpenSessionUnAssosiated() {
         Assert.Throws<ArgumentNullException>(() => {
            var factory = new Caching.Sessions.SessionFactory();
            factory.OpenSession();
         });
      }

      [Fact]
      public void OpenSession() {
         var factory = GetSessionFactory();
         Assert.NotNull(factory.OpenSession());

         var custom = factory.OpenSession(TimeSpan.FromMinutes(5));
         Assert.NotNull(custom);
         Assert.Equal(TimeSpan.FromMinutes(5), custom.CachingPeriod);
         Assert.NotEqual(Guid.Empty, custom.Guid);
      }

      private Caching.Sessions.SessionFactory GetSessionFactory() {
         var factory = new Caching.Sessions.SessionFactory();
         factory.SetMediator(new Mocks.SessionMediatorMock());

         return factory;
      }
   }
}
