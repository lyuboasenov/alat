using System;
using System.Collections.Generic;

namespace Alat.Caching.Tests.Mocks {
   internal class NotifyingCacheStoreMock : CacheStore, IObservable<NotifyingCacheStoreMock.MethodCall> {
      private IList<IObserver<MethodCall>> Observers = new List<IObserver<MethodCall>>();

      public NotifyingCacheStoreMock(IEnumerable<CacheItem> items) { }

      public void Add<T>(string key, T data, string tag, DateTime dateTime) {
         MethodCalled(nameof(Add), key, data.ToString(), tag, dateTime);
      }

      public bool Any() {
         MethodCalled(nameof(Any));
         return true;
      }

      public void Clean() {
         MethodCalled(nameof(Clean));
      }

      public bool Contains(string key) {
         MethodCalled(nameof(Contains), key);
         return default(bool);
      }

      public CacheItem Find<T>(string key) {
         MethodCalled(nameof(Find), key);
         return default(CacheItem);
      }

      public string FindTag(string key) {
         MethodCalled(nameof(FindTag), key);
         return default(string);
      }

      public long GetSize() {
         MethodCalled(nameof(GetSize));
         return default(long);
      }

      public void Remove(string[] key) {
         MethodCalled(nameof(Remove), key);
      }

      public void RemoveAll() {
         MethodCalled(nameof(RemoveAll));
      }

      public void Reset(string key, DateTime dateTime) {
         MethodCalled(nameof(Reset), key, dateTime);
      }

      public IDisposable Subscribe(IObserver<MethodCall> observer) {
         if (!Observers.Contains(observer)) {
            Observers.Add(observer);
         }

         return new Unsubscriber<MethodCall>(Observers, observer);
      }

      private void MethodCalled(string name, params object[] parameters) {
         NotifyObservers(new MethodCall() { MethodName = name, Parameters = parameters });
      }

      private void NotifyObservers(MethodCall parameters) {
         foreach (var observer in Observers) {
            observer.OnNext(parameters);
         }
      }

      private class Unsubscriber<T> : IDisposable {
         private IList<IObserver<T>> observers;
         private IObserver<T> observer;

         public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer) {
            this.observers = observers;
            this.observer = observer;
         }

         public void Dispose() {
            if (observer != null && observers.Contains(observer))
               observers.Remove(observer);
         }
      }

      public class MethodCall {
         public string MethodName { get; set; }
         public object[] Parameters { get; set; }
      }
   }
}
