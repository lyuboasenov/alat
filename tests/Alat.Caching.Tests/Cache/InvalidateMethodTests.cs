using Alat.Caching.Tests.Mocks;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class InvalidateMethodTests : MethodTests {
      [Fact]
      public void Default() {
         Assert.Throws<RemoveAllCalledException>(() =>
            Cache.Invalidate()
         );
      }
   }
}
