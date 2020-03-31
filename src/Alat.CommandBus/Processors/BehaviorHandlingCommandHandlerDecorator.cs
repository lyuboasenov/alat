using Alat.CommandBus.Abstractions;
using Alat.CommandBus.Abstractions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus.Processors {
   internal class BehaviorHandlingCommandHandlerDecorator {
      private IEnumerable<IBehavior<IRequest>> _behaviors;
      private IEnumerable<IBehavior<IRequest>> _preBehaviors;
      private IEnumerable<IBehavior<IRequest>> _postBehaviors;

      public BehaviorHandlingCommandHandlerDecorator(IEnumerable<IBehavior<IRequest>> behaviors) {
         _behaviors = behaviors ?? throw new ArgumentNullException(nameof(behaviors));

         _preBehaviors = _behaviors.
            Where(b =>
               b.GetType().
                  GetInterfaces().
                  Any(i =>
                     i.IsGenericType && i.GetGenericTypeDefinition() != typeof(IPostProcessorBehavior<>)));
         _postBehaviors = _behaviors.Where(b => !_preBehaviors.Contains(b));
      }

      public async Task ExecutePreProcessorsAsync(IRequest request, CancellationToken cancellationToken) {
         foreach(var behavior in _preBehaviors) {
            await behavior.ProcessAsync(request, cancellationToken).ConfigureAwait(false);
         }
      }

      public async Task ExecutePostProcessorsAsync(IRequest request, CancellationToken cancellationToken) {
         foreach (var behavior in _postBehaviors) {
            await behavior.ProcessAsync(request, cancellationToken).ConfigureAwait(false);
         }
      }
   }
}
