using System;

namespace Alat.Caching.Tests.Mocks {
   public class ResetCalledException : Exception {
      public Tuple<string, DateTime> Parameters { get; set; }
         
      public ResetCalledException(Tuple<string, DateTime> tuple) {
         this.Parameters = tuple;
      }
   }
}
