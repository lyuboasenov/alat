using Alat.CommandBus.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.Infrastructure.Commands {
   public abstract class CommandHandler<TParams> : ICommandHandler<TParams> where TParams : IRequest {
      public abstract Task<IResponse> ExecuteAsync(TParams @params, CancellationToken cancellationToken);
   }

   public abstract class CommandHandler<TParams, TResponse> :
      ICommandHandler<TParams, TResponse> where TParams : IRequest<TResponse> {
      public abstract Task<IResponse<TResponse>> ExecuteAsync(TParams @params, CancellationToken cancellationToken);

      async Task<IResponse> ICommandHandler<TParams>.ExecuteAsync(TParams @params, CancellationToken cancellationToken) {
         return await ExecuteAsync(@params, cancellationToken).ConfigureAwait(false);
      }
   }
}
