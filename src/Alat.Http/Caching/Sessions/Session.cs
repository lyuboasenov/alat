using System;

namespace Alat.Http.Caching.Sessions {
   public sealed class Session : IDisposable {
      private bool disposed = false;
      internal Guid Guid { get; } = Guid.NewGuid();
      
      public TimeSpan CachingPeriod { get; }
      private ISessionMediator Mediator { get; }

      internal Session(ISessionMediator mediator, TimeSpan cachePeriod) {
         Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

         if (cachePeriod <= TimeSpan.Zero) {
            throw new ArgumentException(nameof(cachePeriod));
         }
         CachingPeriod = cachePeriod;

         mediator.RegisterSession(this);
      }

      ~Session() {
         Dispose(false);
      }

      public void Abbandon() {
         ValidateNotDisposed();

         Mediator.AbbandonSession(this);
         disposed = true;
      }

      public void Close() {
         ValidateNotDisposed();

         Mediator.CloseSession(this);
         disposed = true;
      }

      public void Dispose() {
         // Dispose of unmanaged resources.
         Dispose(true);
         // Suppress finalization.
         GC.SuppressFinalize(this);
      }

      private void Dispose(bool disposing) {
         if (!disposed) {

            if (disposing) {
               // Free managed resource
               Abbandon();
            }

            disposed = true;
         }
      }

      private void ValidateNotDisposed() {
         if (disposed) {
            throw new ObjectDisposedException("Caching session have already been disposed.");
         }
      }
   }
}