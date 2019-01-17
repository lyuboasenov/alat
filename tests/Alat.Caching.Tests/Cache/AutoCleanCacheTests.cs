using Alat.Caching.Tests.Mocks;
using System;
using System.Threading;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class AutoCleanCacheTests {
      private CacheStore Store { get; }
      private CleanCallObserver CleanCallObserverInstance { get; }
      private IDisposable CleanCallObserverInstanceHandle { get; }

      public AutoCleanCacheTests() {
         var store = new NotifyingCacheStoreMock(CacheItemMock.GetMockItems());
         Store = store;

         CleanCallObserverInstance = new CleanCallObserver();
         CleanCallObserverInstanceHandle = store.Subscribe(CleanCallObserverInstance);
      }

      [Fact]
      public void InitialClean() {
         new Impl.Cache(Store, TimeSpan.FromMilliseconds(500));
         Thread.Sleep(1000);

         Assert.True(CleanCallObserverInstance.IsCleanCalled);
      }

      [Fact]
      public void Recurent() {
         new Impl.Cache(Store, TimeSpan.FromMilliseconds(500));

         Thread.Sleep(1000);
         Assert.True(CleanCallObserverInstance.IsCleanCalled);

         CleanCallObserverInstance.Reset();
         Thread.Sleep(1000);
         Assert.True(CleanCallObserverInstance.IsCleanCalled);
      }

      private class CleanCallObserver : IObserver<NotifyingCacheStoreMock.MethodCall> {
         public bool IsCleanCalled { get; private set; }

         public void Reset() {
            IsCleanCalled = false;
         }

         public void OnCompleted() {
            throw new NotImplementedException();
         }

         public void OnError(Exception error) {
            throw new NotImplementedException();
         }

         public void OnNext(NotifyingCacheStoreMock.MethodCall value) {
            if (value.MethodName == "Clean") {
               IsCleanCalled = true;
            }
         }
      }
   }
}
