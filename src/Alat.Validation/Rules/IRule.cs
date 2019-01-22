namespace Alat.Validation.Rules {
   public interface IRule {
      string ErrorMessage { get; }
      bool IsValid { get; }
      void Validate(string key, object value);
   }
}
