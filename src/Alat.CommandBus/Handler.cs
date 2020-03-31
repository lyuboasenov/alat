using Alat.CommandBus.Abstractions;
using Alat.CommandBus.Abstractions.Behaviors;
using Alat.CommandBus.Processors;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.CommandBus {
   internal sealed class Handler : IHandler {

      private readonly ConcurrentDictionary<Type, object> _requestHandlers = new ConcurrentDictionary<Type, object>();
      private readonly ICommandHandlerFactory _commandHandlerFactory;
      private readonly IBehaviorFactory _behaviorFactory;

      public Handler(ICommandHandlerFactory commandHandlerFactory, IBehaviorFactory behaviorFactory) {
         _commandHandlerFactory = commandHandlerFactory ?? throw new ArgumentNullException(nameof(commandHandlerFactory));
         _behaviorFactory = behaviorFactory ?? throw new ArgumentNullException(nameof(behaviorFactory));
      }

      public async Task<IResponse<TResult>> HandleAsync<TResult>(IRequest<TResult> request, CancellationToken cancellationToken = default) {
         if (request == null) {
            try {
               throw new ArgumentNullException(nameof(request));
            } catch (ArgumentNullException ex) {
               return new Response<TResult>(ex);
            }
         }

         try {
            var handler = (ICommandHandler<IRequest>) _requestHandlers.GetOrAdd(
               request.GetType(),
               t => GetHandler(t));

            var result = await handler.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
            return (IResponse<TResult>) result;
         } catch (Exception ex) {
            return new Response<TResult>(ex);
         }
      }

      public IResponse Handle(IRequest request) {
         var task = HandleAsync(request);
         task.ConfigureAwait(false);
         return task.Result;
      }

      public Task<IResponse> HandleAsync(IRequest request, CancellationToken cancellationToken = default) {
         if (request == null) {
            try {
               throw new ArgumentNullException(nameof(request));
            } catch (ArgumentNullException ex) {
               IResponse result = new Response(ex);
               return Task.FromResult(result);
            }
         }

         try {
            var handler = (ICommandHandler<IRequest>) _requestHandlers.GetOrAdd(
               request.GetType(),
               t => GetHandler(t));

            return handler.ExecuteAsync(request, cancellationToken);
         } catch (Exception ex) {
            IResponse result = new Response(ex);
            return Task.FromResult(result);
         }
      }

      IResponse<TResult> IHandler.Handle<TResult>(IRequest<TResult> request) {
         var task = HandleAsync(request);
         task.ConfigureAwait(false);
         return task.Result;
      }

      private object GetHandler(Type type) {
         var handler = _commandHandlerFactory.GetHandler(type);
         var behaviors = _behaviorFactory.
            GetBehaviors(type)?.
            Select(b => (IBehavior<IRequest>) b) ?? Enumerable.Empty<IBehavior<IRequest>>();

         var handlerType =
            handler.
               GetType().
               GetInterfaces().
               First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
         var requestType = handlerType.GetGenericArguments()[0];

         return Activator.CreateInstance(typeof(CommandHandlerDecorator<>).MakeGenericType(requestType), handler, behaviors);
      }
   }
}
