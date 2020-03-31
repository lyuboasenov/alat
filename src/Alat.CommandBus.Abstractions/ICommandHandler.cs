using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus.Abstractions {
   public interface ICommandHandler<in TRequest> where TRequest : IRequest {
      Task<IResponse> ExecuteAsync(TRequest @params, CancellationToken cancellationToken);
   }

   public interface ICommandHandler<in TRequest, TResult> : ICommandHandler<TRequest> where TRequest : IRequest<TResult> {
      new Task<IResponse<TResult>> ExecuteAsync(TRequest @params, CancellationToken cancellationToken);
   }
}
