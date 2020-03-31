using Alat.CommandBus.Abstractions;
using Alat.CommandBus.Abstractions.Behaviors;
using System;

namespace Alat.CommandBus {
   public static class Setup {
      public static void Initialize(IRegistry registry) {
         registry = registry ?? throw new ArgumentNullException(nameof(registry));

         registry.RegisterType<IHandler, Handler>();
         registry.RegisterType<ICommandHandlerFactory, CommandHandlerFactory>();
         registry.RegisterType<IBehaviorFactory, Behaviors.BehaviorFactory>();
      }
   }
}
