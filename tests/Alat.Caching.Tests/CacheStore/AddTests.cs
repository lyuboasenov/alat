using System;
using System.IO;
using System.Text;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class AddTests : MethodTests, IDisposable {
      private Caching.ICacheStore CacheStore { get; }

      private string StringDataKey { get; } = "string-data-key";
      private string StringDataData { get; } = "string-data-data";
      private string StringDataTags { get; } = "string-data-tags";

      private string EmptyTagKey { get; } = "empty-tag-key";
      private string EmptyTagData { get; } = "empty-tag-data";

      private string StreamDataKey { get; } = "stream-data-key";
      private string StreamDataDataContent { get; } = "stream-data-data-content";
      private Stream StreamDataData { get; }
      private string StreamDataTags { get; } = "stream-data-tags";


      private string ObjectDataKey { get; } = "object-data-key";
      private Custom ObjectDataData { get; }
      private string ObjectDataTags { get; } = "object-data-tags";

      private string NonExistingKey { get; } = "non-existing-key";
      private string EmptyKey { get; } = "";
      private string NullKey { get; } = null;

      private DateTime ExisingExpirationDate { get; } = DateTime.UtcNow.AddMinutes(1);

      protected AddTests() {
         var memoryStream = new MemoryStream();
         var bytes = Encoding.UTF8.GetBytes(StreamDataDataContent);
         memoryStream.Write(bytes, 0, bytes.Length);
         memoryStream.Seek(0, SeekOrigin.Begin);
         StreamDataData = memoryStream;

         ObjectDataData = new Custom() {
            Int = 1,
            CustomObject = new Custom() {
               Int = 2
            }
         };

         CacheStore = CreateCacheStore();
      }

      [Fact]
      public void StringDataCall() {
         CacheStore.Store(CreateCacheItem(StringDataKey, StringDataData, StringDataTags, ExisingExpirationDate));
      }

      [Fact]
      public void StreamDataCall() {
         CacheStore.Store(CreateCacheItem(StreamDataKey, StreamDataData, StreamDataTags, ExisingExpirationDate));
      }

      [Fact]
      public void ObjectDataCall() {
         CacheStore.Store(CreateCacheItem(ObjectDataKey, ObjectDataData, ObjectDataTags, ExisingExpirationDate));
      }

      [Fact]
      public void NullKeyCall() {
         Assert.Throws<ArgumentNullException>(() => 
            CacheStore.Store(CreateCacheItem(NullKey, StringDataData, StringDataTags, ExisingExpirationDate)));
      }

      [Fact]
      public void NullDataCall() {
         Assert.Throws<ArgumentNullException>(() => 
            CacheStore.Store(CreateCacheItem(StringDataKey, NullKey, StringDataTags, ExisingExpirationDate)));
      }

      [Fact]
      public void EmptyKeyCall() {
         Assert.Throws<ArgumentException>(() => 
            CacheStore.Store(CreateCacheItem(EmptyKey, StringDataData, StringDataTags, ExisingExpirationDate)));
      }

      protected abstract ICacheStore CreateCacheStore();

#pragma warning disable CA1034 // Nested types should not be visible
      public class Custom {
#pragma warning restore CA1034 // Nested types should not be visible
         public int Int { get; set; }
         private readonly string String = "private-string";

         public Custom CustomObject { get; set; }

         public Custom() {
            
         }

         public string GetString() { return String; }
      }

      #region IDisposable Support
      private bool disposedValue = false; // To detect redundant calls

      protected virtual void Dispose(bool disposing) {
         if (!disposedValue) {
            if (disposing) {
               StreamDataData.Dispose();
            }
            disposedValue = true;
         }
      }

      ~AddTests() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(false);
      }

      // This code added to correctly implement the disposable pattern.
      public void Dispose() {
         // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
         Dispose(true);
         GC.SuppressFinalize(this);
      }
      #endregion
   }
}
