using System;
using System.IO;
using System.Text;
using Xunit;

namespace Alat.Caching.Tests.CacheStore {
   public abstract class FindTests: MethodTests, IDisposable {
      private Caching.ICacheStore CacheStore { get; }

      private string StringDataKey { get; } = "string-data-key";
      private string StringDataData { get; } = "string-data-data";
      private string StringDataTags { get; } = "string-data-tags";

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

      protected FindTests() {
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

         CacheStore.Store(
            CreateCacheItem(StringDataKey, StringDataData, StringDataTags, ExisingExpirationDate));
         CacheStore.Store(
            CreateCacheItem(StreamDataKey, StreamDataData, StreamDataTags, ExisingExpirationDate));
         CacheStore.Store(
            CreateCacheItem(ObjectDataKey, ObjectDataData, ObjectDataTags, ExisingExpirationDate));
      }

      [Fact]
      public void StringDataCall() {
         var item = CacheStore.Retrieve<string>(StringDataKey);

         Assert.Equal(StringDataKey, item.Meta.Key);
         Assert.Equal(StringDataData, item.Data);
         Assert.Equal(StringDataTags, item.Meta.Tag);
      }

      [Fact]
      public void StreamDataCall() {
         var item = CacheStore.Retrieve<Stream>(StreamDataKey);


         var data = Encoding.UTF8.GetString((item.Data as MemoryStream).ToArray());

         Assert.Equal(StreamDataKey, item.Meta.Key);
         Assert.Equal(StreamDataDataContent, data);
         Assert.Equal(StreamDataTags, item.Meta.Tag);
      }

      [Fact]
      public void ObjectDataCall() {
         var item = CacheStore.Retrieve<Custom>(ObjectDataKey);

         var data = item.Data;

         Assert.Equal(ObjectDataKey, item.Meta.Key);
         Assert.Equal(ObjectDataData.Int, data.Int);
         Assert.Equal(ObjectDataData.GetString(), data.GetString());
         Assert.NotNull(data.CustomObject);
         Assert.Equal(ObjectDataData.CustomObject.Int, data.CustomObject.Int);
         Assert.Equal(ObjectDataData.CustomObject.GetString(), data.CustomObject.GetString());
         Assert.Equal(ObjectDataTags, item.Meta.Tag);
      }

      [Fact]
      public void NonExistingKeyCall() {
         Assert.Null(CacheStore.Retrieve<Custom>(NonExistingKey));
      }

      [Fact]
      public void NullKeyCall() {
         Assert.Throws<ArgumentNullException>(() => CacheStore.Retrieve<string>(NullKey));
      }

      [Fact]
      public void EmptyKeyCall() {
         Assert.Throws<ArgumentException>(() => CacheStore.Retrieve<Stream>(EmptyKey));
      }

      protected abstract Caching.ICacheStore CreateCacheStore();

      public class Custom {
         public int Int { get; set; }
         private string String = "private-string";

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

      ~FindTests() {
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
