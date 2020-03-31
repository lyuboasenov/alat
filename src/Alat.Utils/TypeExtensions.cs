using System;
using System.Linq;

namespace Alat.Utils {
   public static class TypeExtensions {
      public static Type GetGenericTypeDefinition(this Type self, Type genericType) {
         if (genericType.IsInterface) {
            return self?.
               GetInterfaces().
               FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);
         } else {
            var type = self;
            while(type != null && type != typeof(object)) {
               if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType) {
                  return type;
               }
            }
         }

         return null;
      }
   }
}
