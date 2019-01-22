namespace Alat.Validation {
   public interface IValidationContextFactory {
      IValidationContext GetContextFor(IValidatable target, bool autoValidate = false);
   }
}
