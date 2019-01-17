namespace Alat.Validation {
   public interface ValidationContextFactory {
      ValidationContext GetContextFor(Validatable target, bool autoValidate = false);
   }
}
