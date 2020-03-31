using System;

namespace Alat.CommandBus.Abstractions {
   public interface IRegistry {
      void RegisterType<T1, T2>() where T2 : T1;
   }
}
