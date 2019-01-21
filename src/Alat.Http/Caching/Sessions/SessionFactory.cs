using System;

namespace Alat.Http.Caching.Sessions {
   public sealed class SessionFactory {
      private ISessionMediator mediator;

      public Session OpenSession() {
         ValidateMediatorNotNull();

         return OpenSession(mediator.DefaultCachePeriod);
      }

      public Session OpenSession(TimeSpan cachePeriod) {
         ValidateMediatorNotNull();

         return new Session(mediator, cachePeriod);
      }

      internal void SetMediator(ISessionMediator mediator) {
         this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
      }

      private void ValidateMediatorNotNull() {
         if (null == mediator) {
            throw new ArgumentNullException("Factory is not assosiated with caching handler");
         }
      }
   }
}