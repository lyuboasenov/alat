using System;
using System.Collections.Generic;
using Alat.Abstractions.IoC;
using Alat.Abstractions.Mapping;
using Alat.Abstractions.System;
using Alat.Infrastructure.Services.FileSystem;
using Alat.Infrastructure.Services.Mapping;

namespace Alat.Infrastructure {
   public static class Setup {
      public static void Initialize(IRegistry registry) {
         registry = registry ?? throw new ArgumentNullException(nameof(registry));

         RegisterMapper(registry);

         RegisterServices(registry);

         RegisterCommandBus(registry);
      }

      private static void RegisterCommandBus(IRegistry registry) {
         CommandBus.Setup.Initialize(new CommandBusRegistryAdapter(registry));
         registry.RegisterType<CommandBus.Abstractions.IResolver, CommandBusResolverAdapter>();
      }

      private static void RegisterServices(IRegistry registry) {
         registry.RegisterInstance(FileSystem.GetFile());
         registry.RegisterInstance(FileSystem.GetDirectory());

         registry.RegisterInstance<IGuid>(new Services.System.Guid());
         registry.RegisterInstance<IDateTime>(new Services.System.DateTime());
      }

      private static void RegisterMapper(IRegistry registry) {
         registry.RegisterType<AutoMapper.IConfigurationProvider, AutomapperConfiguration>();
         registry.RegisterType<AutoMapper.IMapper, AutoMapper.Mapper>();
         registry.RegisterType<IMapper, Mapper>();
      }

      private class CommandBusRegistryAdapter : CommandBus.Abstractions.IRegistry {
         private readonly IRegistry _registry;

         public CommandBusRegistryAdapter(IRegistry registry) {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
         }

         public void RegisterType<T1, T2>() where T2 : T1 {
            _registry.RegisterType<T1, T2>();
         }
      }

      private class CommandBusResolverAdapter : CommandBus.Abstractions.IResolver {
         private readonly IResolver _resolver;

         public CommandBusResolverAdapter(IResolver resolver) {
            _resolver = resolver;
         }

         public bool IsTypeRegistered(Type type) {
            return _resolver.IsTypeRegistered(type);
         }

         public object Resolve(Type t) {
            return _resolver.Resolve(t);
         }
      }

      private class AutomapperConfiguration : AutoMapper.MapperConfiguration {
         public AutomapperConfiguration(IResolver resolver) : base(cfg =>
                    cfg.AddProfiles(resolver.Resolve(typeof(IEnumerable<AutoMapper.Profile>)) as IEnumerable<AutoMapper.Profile>)) {

         }
      }
   }
}
