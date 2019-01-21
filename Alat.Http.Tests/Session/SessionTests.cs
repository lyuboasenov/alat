using System;
using Xunit;

namespace Alat.Http.Tests.Session {
   public class SessionTests {
      [Fact]
      public void CreateInstance() {
         var mediator = new Mocks.SessionMediatorMock();
         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));
      }

      [Fact]
      public void CreateInstanceNullMediator() {
         Assert.Throws<ArgumentNullException>(() => new Caching.Sessions.Session(null, TimeSpan.FromMinutes(5)));
      }

      [Fact]
      public void CreateInstanceZeroCachingPeriod() {
         var mediator = new Mocks.SessionMediatorMock();
         Assert.Throws<ArgumentException>(() => new Caching.Sessions.Session(mediator, TimeSpan.Zero));
      }

      [Fact]
      public void CreateInstanceNegCachingPeriod() {
         var mediator = new Mocks.SessionMediatorMock();
         Assert.Throws<ArgumentException>(() => new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(-1)));
      }

      [Fact]
      public void RegisterSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var observer = new Testing.Mocks.NotifyingMockObserver();
         mediator.Subscribe(observer);

         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));

         Assert.True(observer.IsMethodCalled("RegisterSession", session));
      }

      [Fact]
      public void DisposeSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var observer = new Testing.Mocks.NotifyingMockObserver();
         mediator.Subscribe(observer);

         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));
         Assert.True(observer.IsMethodCalled("RegisterSession", session));
         Assert.False(observer.IsMethodCalled("CloseSession", session));
         session.Dispose();

         Assert.True(observer.IsMethodCalled("AbbandonSession", session));
      }

      [Fact]
      public void CloseSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var observer = new Testing.Mocks.NotifyingMockObserver();
         mediator.Subscribe(observer);

         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));
         Assert.True(observer.IsMethodCalled("RegisterSession", session));
         Assert.False(observer.IsMethodCalled("CloseSession", session));
         session.Close();
         Assert.True(observer.IsMethodCalled("CloseSession", session));
      }

      [Fact]
      public void AbbandonSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var observer = new Testing.Mocks.NotifyingMockObserver();
         mediator.Subscribe(observer);

         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));
         Assert.True(observer.IsMethodCalled("RegisterSession", session));
         Assert.False(observer.IsMethodCalled("AbbandonSession", session));
         session.Abbandon();
         Assert.True(observer.IsMethodCalled("AbbandonSession", session));
      }

      [Fact]
      public void CloseAfterDisposeSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));

         session.Dispose();
         Assert.Throws<ObjectDisposedException>(() => session.Close());
      }

      [Fact]
      public void AbbandonAfterDisposeSession() {
         var mediator = new Mocks.SessionMediatorMock();
         var session = new Caching.Sessions.Session(mediator, TimeSpan.FromMinutes(5));

         session.Dispose();
         Assert.Throws<ObjectDisposedException>(() => session.Abbandon());
      }
   }
}
