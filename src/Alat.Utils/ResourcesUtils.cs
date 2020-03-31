using System.IO;
using System.Reflection;

namespace Alat.PSSp.Utils {
   public static class ResourcesUtils {
      public static string GetResourceString(Assembly assembly, string id) {
         using (Stream stream = assembly.GetManifestResourceStream(id))
         using (var reader = new StreamReader(stream)) {
            return reader.ReadToEnd();
         }
      }
   }
}
