using Alat.Patterns.Observer;
using System;
using System.Collections.Generic;

namespace Alat.Testing.Mocks {
   public abstract class NotifyingMock : IObservable<MethodCall> {
      private IList<IObserver<MethodCall>> observers;

      protected NotifyingMock() {
         observers = new List<IObserver<MethodCall>>();
      }

      public IDisposable Subscribe(IObserver<MethodCall> observer) {
         if (!observers.Contains(observer)) {
            observers.Add(observer);
         }

         return new Unsubscriber<IObserver<MethodCall>>(observers, observer);
      }

      protected void MethodCalled(string name, params object[] parameters) {
         NotifyObservers(new MethodCall(name, parameters));
      }

      private void NotifyObservers(MethodCall parameters) {
         foreach (var observer in observers) {
            observer.OnNext(parameters);
         }
      }
   }
}
