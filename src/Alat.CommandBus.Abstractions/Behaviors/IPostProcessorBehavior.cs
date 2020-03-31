namespace Alat.CommandBus.Abstractions.Behaviors {
   public interface IPostProcessorBehavior<TParam> : IBehavior<TParam> where TParam : IRequest {
   }
}
