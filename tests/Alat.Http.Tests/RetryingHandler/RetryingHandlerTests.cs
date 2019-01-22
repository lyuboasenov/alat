using System;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Alat.Http.Tests.RetryingHandler {
   public class RetryingHandlerTests {
      
      [Fact]
      public void RetrySettingTest() {
         var handler = new Http.RetryingHandler(4);

         Assert.Equal(4, handler.Retry);
         Assert.NotNull(handler.InnerHandler);
      }

      [Fact]
      public void RetrySettingNegativeTest() {
         Assert.Throws<ArgumentException>(() => new Http.RetryingHandler(-4));
      }


      [Fact]
      public void NullInnerHanlderTest() {
         Assert.Throws<ArgumentNullException>(() => new Http.RetryingHandler(4, null));
      }

      [Fact]
      public void SuccessfullCallWithInteruptions() {
         var client = CreateClient(4, 2);
         var response = client.GetAsync("http://dummy").Result;

         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      }

      [Fact]
      public void UnsuccessfullCallWithInteruptions() {
         var client = CreateClient(4, 1);
         Assert.Throws<AggregateException>(() => client.GetAsync("http://dummy").Result);
      }

      private static HttpClient CreateClient(int retry, int throwOn) {
         return new HttpClient(new Http.RetryingHandler(retry, CreateThrowingHandler(throwOn)));
      }

      private static HttpMessageHandler CreateThrowingHandler(int throwOn) {
         return new Mocks.ThrowingHttpHandler(throwOn);
      }
   }
}
