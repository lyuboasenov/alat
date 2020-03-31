using System;
using System.Collections.Generic;

namespace Alat.CommandBus.Abstractions.Behaviors {
   public interface IBehaviorFactory {
      IEnumerable<IBehavior<TRequest>> GetBehaviors<TRequest>() where TRequest : IRequest;
      IEnumerable<object> GetBehaviors(Type type);
   }
}
