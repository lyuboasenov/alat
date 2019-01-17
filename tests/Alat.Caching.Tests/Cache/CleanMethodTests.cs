using Alat.Caching.Tests.Mocks;
using Xunit;

namespace Alat.Caching.Tests.Cache {
   public class CleanMethodTests : MethodTests {
      [Fact]
      public void Default() {
         Assert.Throws<CleanCalledException>(() =>
            Cache.Clean()
         );
      }
   }
}
