namespace Alat.Testing.Mocks {
   public class MethodCall {
      public string MethodName { get; set; }
#pragma warning disable CA1819 // Properties should not return arrays
      public object[] Parameters { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

      public MethodCall(string name) {
         MethodName = name;
      }

      public MethodCall(string name, params object[] parameters) {
         MethodName = name;
         Parameters = parameters;
      }
   }
}
