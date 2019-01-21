using Alat.Http.Caching.Sessions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Alat.Http.Caching {
   public sealed class CachingHandler : DelegatingHandler {
      public SessionFactory SessionFactory { get; private set; }

      private ICache Cache { get; set; }
      private SessionMediator Mediator { get; set; }

      public CachingHandler(Settings settings) : 
         this(new SessionFactory(), settings, new HttpClientHandler()) {
      }

      public CachingHandler(SessionFactory sessionFactory, Settings settings) : 
         this(sessionFactory, settings, new HttpClientHandler()) {
      }

      public CachingHandler(SessionFactory sessionFactory, Settings settings,
         HttpMessageHandler innerHandler) : base(innerHandler) {
         if (null == settings) {
            throw new ArgumentNullException(nameof(settings));
         }

         SessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
         Mediator = new SessionMediator(settings.Cache, settings.CachingEnabledByDefault, settings.CachePeriod);
         SessionFactory.SetMediator(Mediator);

         UpdateSettings(settings);
      }

      public void UpdateSettings(Settings settings) {
         if (null == settings) {
            throw new ArgumentNullException(nameof(settings));
         }

         Cache = settings.Cache ?? throw new ArgumentNullException(nameof(settings.Cache));

         if (settings.CachePeriod <= TimeSpan.Zero) {
            throw new ArgumentException(nameof(settings.CachePeriod));
         }

         Mediator.Update(settings.Cache, settings.CachingEnabledByDefault, settings.CachePeriod);
      }

      protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, 
         CancellationToken cancellationToken) {
         HttpResponseMessage response = null;

         if (Mediator.ShouldCache()) {
            string requestHash = request.ToString().GetHashCode().ToString();

            if (!Cache.Contains(requestHash)) {
               await StoreRequestAsync(requestHash, request, cancellationToken);
            }

            response = RetrieveResponse(requestHash);
         } else {
            response = await base.SendAsync(request, cancellationToken);
         }

         return response;
      }

      private async Task StoreRequestAsync(string requestHash, HttpRequestMessage request, CancellationToken cancellationToken) {
         using (var response = await base.SendAsync(request, cancellationToken)) {
            response.EnsureSuccessStatusCode();

            Mediator.OnKeyAdded(requestHash);
            await CacheResponseAsync(requestHash, response);
         }
      }

      private HttpResponseMessage RetrieveResponse(string requestUri) {
         HttpResponseMessage response = null;

         if (Cache.Contains(requestUri)) {
            var stream = Cache.Retrieve<Stream>(requestUri);
            response = new HttpResponseMessage(System.Net.HttpStatusCode.OK) {
               Content = new StreamContent(stream)
            };
         } else {
            throw new ArgumentException($"Response with uri: {requestUri} not found in cache.");
         }

         return response;
      }

      private async Task CacheResponseAsync(string requestUri, HttpResponseMessage response) {
         using (var contentStream = await response.Content.ReadAsStreamAsync()) {
            Cache.Store(requestUri, contentStream, Mediator.GetCachingPeriod());
         }
      }
   }
}
