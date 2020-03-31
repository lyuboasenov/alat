using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus.Abstractions.Behaviors {
   public interface IBehavior<TParam> where TParam : IRequest {
      Task ProcessAsync(TParam param, CancellationToken cancellationToken);
   }
}
