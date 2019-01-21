using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.Http {
   public class RetryingHandler : DelegatingHandler {

      public int Retry { get; private set; }

      public RetryingHandler(int retry) : base(new HttpClientHandler()) {
         Initialize(retry);
      }

      public RetryingHandler(int retry, HttpMessageHandler innerHandler) : base(innerHandler) {
         if (null == innerHandler) {
            throw new ArgumentNullException(nameof(innerHandler));
         }
         Initialize(retry);
      }

      private void Initialize(int retry) {
         if (retry <= 0) {
            throw new ArgumentException(nameof(retry));
         }

         Retry = retry;
      }

      protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
         IList<Exception> exceptions = null;
         int iteration = 0;
         do {
            try {
               return await base.SendAsync(request, cancellationToken);
            } catch (HttpRequestException ex) {
               if (null == exceptions) {
                  exceptions = new List<Exception>();
               }
               exceptions.Add(ex);
               iteration++;
               await Task.Delay(1000);
            }
         } while (iteration < Retry);

         throw new AggregateException(exceptions);
      }
   }
}
