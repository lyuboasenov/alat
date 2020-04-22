using System.Collections;
using System.Linq;
using System.Reflection;

namespace Alat.Utils {
   public static class ObjectExtensions {
      private static readonly System.Type _ienumerableType = typeof(IEnumerable);
      public static int GetReflectionHashCode(this object self) {
         var hash = 0;
         if (self != null) {
            foreach (var property in self.
               GetType().
               GetProperties(BindingFlags.Public
                  | BindingFlags.NonPublic
                  | BindingFlags.Instance
                  | BindingFlags.DeclaredOnly).
               Where(p => p.CanRead && (!p.GetIndexParameters()?.Any() ?? true))) {
               hash ^= property.GetValue(self)?.GetHashCode() ?? 0;
            }
         }
         return hash;
      }

      public static bool ReflectionEquals(this object self, object obj) {
         if (self is null || obj is null) {
            return self == obj;
         } else if (self.GetType().Equals(obj.GetType())) {

            foreach (var property in self.
               GetType().
               GetProperties(BindingFlags.Public
                  | BindingFlags.NonPublic
                  | BindingFlags.Instance
                  | BindingFlags.DeclaredOnly).
               Where(p => p.CanRead && (!p.GetIndexParameters()?.Any() ?? true))) {
               var selfValue = property.GetValue(self);
               var objValue = property.GetValue(obj);
               var areEqual = true;

               if (selfValue is null || objValue is null) {
                  return selfValue == objValue;
               } else if (_ienumerableType.IsAssignableFrom(selfValue.GetType()) && !(selfValue is string)) {
                  var selfEnumerator = (selfValue as IEnumerable).GetEnumerator();
                  var objEnumerator = (objValue as IEnumerable).GetEnumerator();

                  while (true) {
                     bool selfHasNext = selfEnumerator.MoveNext(),
                     objHasNext = objEnumerator.MoveNext();

                     if (selfHasNext != objHasNext) {
                        areEqual = false;
                        break;
                     } else if (!selfHasNext) {
                        break;
                     } else if (!Equals(selfEnumerator.Current, objEnumerator.Current)) {
                        areEqual = false;
                        break;
                     }
                  }
               } else {
                  areEqual = Equals(selfValue, objValue);
               }

               if (!areEqual) {
                  return false;
               }
            }

            return true;
         } else {
            return false;
         }
      }
   }
}
