namespace Alat.Validation.Rules {
   public interface Rule {
      string ErrorMessage { get; }
      bool IsValid { get; }
      void Validate(string key, object value);
   }
}
