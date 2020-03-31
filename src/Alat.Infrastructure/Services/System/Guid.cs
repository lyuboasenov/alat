using Alat.Abstractions.System;

namespace Alat.Infrastructure.Services.System {
   public class Guid : IGuid {
      public global::System.Guid New() {
         return global::System.Guid.NewGuid();
      }
   }
}
