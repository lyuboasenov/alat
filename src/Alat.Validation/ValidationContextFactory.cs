namespace Alat.Validation {
   public class ValidationContextFactory : IValidationContextFactory {
      public IValidationContext GetContextFor(IValidatable target, bool autoValidate = false) {
         var context = new ValidationContext(target, autoValidate);
         target.InitializeValidationRules(context);

         return context;
      }
   }
}
