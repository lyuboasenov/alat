using System;
using System.Collections.Generic;

namespace Alat.CommandBus.Abstractions {
   public interface IResponse {
      IEnumerable<Exception> Errors { get; }
      bool IsFault { get; }
      bool IsSuccessful { get; }
   }

   public interface IResponse<out TResult> : IResponse {
      TResult Result { get; }
   }
}
