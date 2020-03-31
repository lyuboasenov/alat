using Alat.CommandBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.CommandBus {
   internal sealed class CommandHandlerFactory : ICommandHandlerFactory {
      private readonly IResolver _resolver;

      public CommandHandlerFactory(IResolver resolver) {
         _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
      }

      public ICommandHandler<TRequest> GetHandler<TRequest>() where TRequest : IRequest {
         return (ICommandHandler<TRequest>) GetHandler(typeof(TRequest));
      }

      public ICommandHandler<TRequest, TResponse> GetHandler<TRequest, TResponse>() where TRequest : IRequest<TResponse> {
         return (ICommandHandler<TRequest, TResponse>) GetHandler(typeof(TRequest));
      }

      public object GetHandler(Type type) {
         var handler = _resolver.IsTypeRegistered(type) ? _resolver.Resolve(type) : GetDefault(type);
         if (handler is null && typeof(IRequest).IsAssignableFrom(type)) {
            foreach (Type requestType in GetHandlerTypeWithCommandVariations(type)) {
               handler = _resolver.Resolve(requestType);
               if (handler != null) {
                  break;
               }
            }
         }

         return handler;
      }

      private static object GetDefault(Type type) {
         if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) {
            type = type.GetGenericArguments()[0];
            return Array.CreateInstance(type, 0);
         } else {
            return default(Type);
         }
      }

      private static IEnumerable<Type> GetHandlerTypeWithCommandVariations(Type requestType) {
         if (typeof(IRequest).IsAssignableFrom(requestType)) {
            var responseType = requestType.
               GetInterfaces().
               FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
            if (requestType is null) {
               foreach (var commandVariation in GetCommandTypeVariations(requestType, typeof(IRequest))) {
                  yield return
                     typeof(ICommandHandler<>).
                     MakeGenericType(
                        commandVariation);
               }
            } else {
               foreach (var commandVariation in GetCommandTypeVariations(requestType, responseType)) {
                  yield return
                     typeof(ICommandHandler<,>).
                     MakeGenericType(
                        commandVariation,
                        responseType.GetGenericArguments()[0]);
               }
            }
         }
      }

      private static IEnumerable<Type> GetCommandTypeVariations(Type type, Type requestBaseType) {
         var classType = type.BaseType;

         while (requestBaseType.IsAssignableFrom(classType)) {
            if (typeof(IRequest).IsAssignableFrom(classType)) {
               yield return classType;
            }

            classType = classType.BaseType;
         }

         var interfaces = new List<Type>();
         interfaces.AddRange(type.GetInterfaces());

         for (int i = 0; i < interfaces.Count; i++) {
            var @interface = interfaces[i];
            if (@interface != requestBaseType && requestBaseType.IsAssignableFrom(@interface)) {
               yield return @interface;

               var baseInterfaces = @interface.GetInterfaces();
               if (baseInterfaces?.Any() ?? false) {
                  interfaces.AddRange(baseInterfaces);
               }
            }
         }
      }
   }
}
