using System;

namespace Alat.Abstractions.IoC {
   public interface IResolver {
      bool IsTypeRegistered(Type type);
      bool IsTypeRegistered<T>();
      T Resolve<T>();
      object Resolve(Type t);
      Type ResolveType<T>();
      Type ResolveType(Type t);
   }
}
