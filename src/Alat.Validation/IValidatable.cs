namespace Alat.Validation {
   public interface IValidatable {
      IValidationContext ValidationContext { get; }
      void OnValidationContextChanged();
      void InitializeValidationRules(IValidationContext validationContext);
   }
}
