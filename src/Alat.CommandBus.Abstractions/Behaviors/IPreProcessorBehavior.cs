namespace Alat.CommandBus.Abstractions.Behaviors {
   public interface IPreProcessorBehavior<TParam> : IBehavior<TParam> where TParam : IRequest {
   }
}
