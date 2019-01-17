namespace Alat.Validation {
   public interface Validatable {
      ValidationContext ValidationContext { get; }
      void OnValidationContextChanged();
      void InitializeValidationRules(ValidationContext validationContext);
   }
}
