using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.Http.Tests.Mocks {
   public class ThrowingHttpHandler : HttpMessageHandler {
      private readonly int throwOnRequestNumber;
      private int requestCounter = 1;

      public ThrowingHttpHandler(int throwOnRequestNumber) {
         this.throwOnRequestNumber = throwOnRequestNumber;
      }

      protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
         if (requestCounter % throwOnRequestNumber == 0) {
            requestCounter++;
            throw new HttpRequestException();
         }

         requestCounter++;
         var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
            Content = new StringContent("All good in the hood tonight.")
         };

         return Task.FromResult(response);
      }
   }
}
