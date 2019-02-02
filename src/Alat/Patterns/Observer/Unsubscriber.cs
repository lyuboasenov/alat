
using System;
using System.Collections.Generic;

namespace Alat.Patterns.Observer {
   public sealed class Unsubscriber<TObserver> : IDisposable {
      private IList<TObserver> observers;
      private readonly TObserver observer;

      public Unsubscriber(IList<TObserver> observers, TObserver observer) {
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
