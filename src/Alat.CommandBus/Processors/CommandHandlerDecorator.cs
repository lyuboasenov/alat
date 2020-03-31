using Alat.CommandBus.Abstractions;
using Alat.CommandBus.Abstractions.Behaviors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus.Processors {
   internal sealed class CommandHandlerDecorator<TRequest> :
      BehaviorHandlingCommandHandlerDecorator,
      ICommandHandler<IRequest> where TRequest : IRequest {

      private readonly ICommandHandler<TRequest> _innerHandler;

      public CommandHandlerDecorator(ICommandHandler<TRequest> innerHandler,
         IEnumerable<IBehavior<IRequest>> behaviors) : base(behaviors) {
         _innerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
      }

      public async Task<IResponse> ExecuteAsync(IRequest @params, CancellationToken cancellationToken) {
         await ExecutePreProcessorsAsync(@params, cancellationToken).ConfigureAwait(false);

         var result = await _innerHandler.ExecuteAsync((TRequest) @params, cancellationToken).ConfigureAwait(false);

         await ExecutePostProcessorsAsync(@params, cancellationToken).ConfigureAwait(false);

         return result;
      }
   }
}
