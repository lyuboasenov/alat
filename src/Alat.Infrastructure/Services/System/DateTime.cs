using Alat.Abstractions.System;

namespace Alat.Infrastructure.Services.System {
   internal class DateTime : IDateTime {
      public global::System.DateTime Now() {
         return global::System.DateTime.Now;
      }
   }
}
