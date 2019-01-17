using System;

namespace Alat.Validation.Rules {
   public class CustomRule : Rule {
      public string ErrorMessage { get; private set; }
      public bool IsValid { get; private set; }
      private Func<string, object, bool> ValidateDelegate { get; set; }

      public CustomRule(string errorMessage, Func<string, object, bool> validate) {
         ErrorMessage = errorMessage;
         ValidateDelegate = validate;
      }

      public void Validate(string key, object value) {
         IsValid = ValidateDelegate(key, value);
      }
   }
}
