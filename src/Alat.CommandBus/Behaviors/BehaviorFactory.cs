using Alat.CommandBus.Abstractions;
using Alat.CommandBus.Abstractions.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.CommandBus.Behaviors {
   internal sealed class BehaviorFactory : IBehaviorFactory {
      private readonly IResolver _resolver;

      public BehaviorFactory(IResolver resolver) {
         _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
      }

      public IEnumerable<IBehavior<TRequest>> GetBehaviors<TRequest>() where TRequest : IRequest {
         return GetBehaviors(typeof(TRequest)).Select(b => (IBehavior<TRequest>) b);
      }

      public IEnumerable<object> GetBehaviors(Type type) {
         var behaviors = _resolver.IsTypeRegistered(type) ? _resolver.Resolve(typeof(IEnumerable<>).MakeGenericType(type)) : null;
         if (behaviors is null && IsRequestType(type)) {
            foreach (Type requestType in GetRequestTypeVariations(type)) {
               behaviors = _resolver.Resolve(typeof(IEnumerable<>).MakeGenericType(requestType));
               if (behaviors != null) {
                  break;
               }
            }
         }

         return behaviors as IEnumerable<object>;
      }

      private static bool IsRequestType(Type type) {
         return type is IRequest;
      }

      private static IEnumerable<Type> GetRequestTypeVariations(Type type) {
         var classType = type.BaseType;

         while (classType != typeof(object)) {
            if (IsRequestType(classType)) {
               yield return classType;
            }

            classType = classType.BaseType;
         }

         var interfaces = new List<Type>();
         interfaces.AddRange(type.GetInterfaces());

         for (int i = 0; i < interfaces.Count; i++) {
            var @interface = interfaces[i];
            if (@interface != typeof(IRequest) && IsRequestType(@interface)) {
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
