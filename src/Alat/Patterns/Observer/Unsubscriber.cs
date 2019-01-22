
using System;
using System.Collections.Generic;

namespace Alat.Patterns.Observer {
   public sealed class Unsubscriber<T> : IDisposable {
      private IList<IObserver<T>> observers;
      private readonly IObserver<T> observer;

      public Unsubscriber(IList<IObserver<T>> observers, IObserver<T> observer) {
         this.observers = observers;
         this.observer = observer;
      }

#pragma warning disable CA1063 // Implement IDisposable Correctly
      public void Dispose() {
#pragma warning restore CA1063 // Implement IDisposable Correctly
         if (observer != null && observers.Contains(observer))
            observers.Remove(observer);
      }
   }
}
