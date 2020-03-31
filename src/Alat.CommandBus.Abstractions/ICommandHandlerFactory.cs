using Alat.CommandBus.Abstractions;
using System;

namespace Alat.CommandBus {
   public interface ICommandHandlerFactory {
      object GetHandler(Type type);
      ICommandHandler<TRequest> GetHandler<TRequest>() where TRequest : IRequest;
      ICommandHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse>;
   }
}