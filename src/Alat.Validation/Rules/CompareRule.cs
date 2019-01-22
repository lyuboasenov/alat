using System.Collections.Generic;
using System.Linq;

namespace Alat.Validation.Rules {
   public class CompareRule : IRule {
      public string ErrorMessage { get; private set; }
      public bool IsValid { get; private set; }

      private Dictionary<string, object> Values { get; set; }

      public CompareRule(string errorMessage) {
         ErrorMessage = errorMessage;
      }

      public void Validate(string key, object value) {
         if (null == Values) {
            Values = new Dictionary<string, object>();
         }
         Values[key] = value;
         var values = Values.Values;

         var result = values.Count < 2;
         if (!result) {
            result = values.Count == values.Distinct().Count();
         }

         IsValid = result;
      }
   }
}
