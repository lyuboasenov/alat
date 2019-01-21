using System;
using System.IO;

namespace Alat.Caching {
   public sealed class StreamCacheItem : IDisposable {
      private bool disposedValue = false; // To detect redundant calls

      public CacheItemMeta Meta { get; set; }
      private bool HandleStreamDisposal { get; set; } = true;

      /// <summary>
      /// Data in stream format
      /// </summary>
      public Stream Stream { get; set; }

      private StreamCacheItem() {

      }

      ~StreamCacheItem() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(false);
      }

      public void Dispose() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      internal static StreamCacheItem FromCacheItem(CacheItemMeta meta, Stream stream, bool handleStreamDisposal) {
         return new StreamCacheItem() {
            HandleStreamDisposal = handleStreamDisposal,
            Meta = meta,
            Stream = stream
         };
      }

      private void Dispose(bool disposing) {
         if (!disposedValue) {
            if (disposing && HandleStreamDisposal) {
               Stream.Close();
            }

            disposedValue = true;
         }
      }
   }
}
