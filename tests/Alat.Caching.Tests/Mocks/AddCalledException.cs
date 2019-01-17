using System;

namespace Alat.Caching.Tests.Mocks {
   public class AddCalledException : Exception {
      public Tuple<string, string, string, DateTime> Parameters { get; set; }

      public AddCalledException(Tuple<string, string, string, DateTime> tuple) {
         this.Parameters = tuple;
      }
   }
}
