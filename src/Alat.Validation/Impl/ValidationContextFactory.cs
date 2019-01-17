namespace Alat.Validation.Impl {
   public class ValidationContextFactory : Validation.ValidationContextFactory {
      public Validation.ValidationContext GetContextFor(Validatable target, bool autoValidate = false) {
         var context = new ValidationContext(target, autoValidate);
         target.InitializeValidationRules(context);

         return context;
      }
   }
}
