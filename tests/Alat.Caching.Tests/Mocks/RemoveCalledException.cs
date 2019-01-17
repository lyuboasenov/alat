using System;

namespace Alat.Caching.Tests.Mocks {
   public class RemoveCalledException : Exception {
      public string[] Parameters;

      public RemoveCalledException(string[] key) {
         this.Parameters = key;
      }
   }
}
