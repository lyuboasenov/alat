using System;

namespace Alat.CommandBus.Abstractions {
   public interface IResolver {
      bool IsTypeRegistered(Type type);
      object Resolve(Type t);
   }
}
