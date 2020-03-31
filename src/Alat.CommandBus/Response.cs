using Alat.CommandBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.CommandBus {
   public class Response : IResponse {
      private readonly List<Exception> _errors = new List<Exception>();

      public Response() {

      }

      public Response(Exception ex) {
         AddError(ex);
      }

      public Response(IResponse response) {
         _errors.AddRange(response.Errors ?? Enumerable.Empty<Exception>());
      }

      public IEnumerable<Exception> Errors { get { return _errors; } }
      public bool IsFault { get; private set; }
      public bool IsSuccessful { get { return !IsFault; } }

      public void AddError(Exception ex) {
         _errors.Add(ex);
         IsFault = true;
      }

      public void Update(IResponse response) {
         _errors.AddRange(response.Errors ?? Enumerable.Empty<Exception>());
      }
   }

   public class Response<TResult> : Response, IResponse<TResult> {
      public Response() {

      }

      public Response(Exception ex) : base(ex) {

      }

      public Response(IResponse<TResult> response) : base(response) {

      }

      public TResult Result { get; set; }
   }
}
