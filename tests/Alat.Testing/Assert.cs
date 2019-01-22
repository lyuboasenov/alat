using Alat.Caching.Tests.Mocks;
using System;

namespace Alat.Testing {
   public static class Assert {

      public static void MethodCalled(Action action, string expectedMethodName) {
         try {
            action();
         } catch (MethodCalledException ex) {
            Xunit.Assert.Equal(expectedMethodName, ex.Name);
         }
      }

      public static void MethodCalled(Action action, string expectedMethodName, params object[] parameters) {
         try {
            action();
         } catch(MethodCalledException ex) {
            Xunit.Assert.Equal(expectedMethodName, ex.Name);
            Xunit.Assert.Equal(parameters, ex.Parameters);
         }
      }
   }
}
