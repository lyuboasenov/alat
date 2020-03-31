using System;

namespace Alat.Abstractions.IoC {
   public interface IRegistry {
      void RegisterType<T1, T2>() where T2 : T1;
      void RegisterType(Type src, Type dest);
      void RegisterInstance(Type type, object @object);
      void RegisterInstance<TInterface>(TInterface @object);
      void RegisterInstance<TInterface>(Func<TInterface> factory);
   }
}
