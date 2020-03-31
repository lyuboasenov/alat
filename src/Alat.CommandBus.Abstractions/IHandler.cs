using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus.Abstractions {
   public interface IHandler {
      IResponse Handle(IRequest request);
      IResponse<TResult> Handle<TResult>(IRequest<TResult> request);
      Task<IResponse> HandleAsync(IRequest request, CancellationToken cancellationToken);
      Task<IResponse<TResult>> HandleAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken);
   }
}
