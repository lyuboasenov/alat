using System;

namespace Alat.Caching.Tests.Mocks {
   public abstract class ThrowingMock {

      protected ThrowingMock() {
      }

#pragma warning disable CA1822 // Mark members as static
      protected void MethodCalled(string name, params object[] parameters) {
#pragma warning restore CA1822 // Mark members as static
         throw new MethodCalledException(name, parameters);
      }

      protected void Throw(Exception ex) {
         throw ex;
      }
   }
}
