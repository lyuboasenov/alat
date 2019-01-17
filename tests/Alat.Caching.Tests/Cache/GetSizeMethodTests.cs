using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class GetSizeMethodTests : MethodTests {

      [Fact]
      public void Default() {
         Assert.Equal(42, Cache.GetCacheSize());
      }
   }
}
