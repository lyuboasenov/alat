using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Caching {
   public class KeyValidator {
      private readonly static KeyValidator instance = new KeyValidator();

      public static KeyValidator GetValidator() {
         return instance;
      }

      private KeyValidator() {

      }

      public void ValidateKey(string key) {
         if (null == key) {
            throw new ArgumentNullException(nameof(key));
         }

         if (string.IsNullOrEmpty(key)) {
            throw new ArgumentException($"{nameof(key)} is empty");
         }

         if (string.IsNullOrWhiteSpace(key)) {
            throw new ArgumentException($"{nameof(key)} is whitespace");
         }
      }

      public void ValidateKeys(IEnumerable<string> keys) {
         if (null == keys) {
            throw new ArgumentNullException(nameof(keys));
         }

         if (!keys.Any()) {
            throw new ArgumentException($"{nameof(keys)} is empty");
         }

         foreach (var key in keys) {
            ValidateKey(key);
         }
      }
   }
}
