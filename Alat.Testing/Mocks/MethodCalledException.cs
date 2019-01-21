using System;
using System.Runtime.Serialization;

namespace Alat.Caching.Tests.Mocks {
   [Serializable]
   public class MethodCalledException : Exception {
      public string Name { get; }
#pragma warning disable CA2235 // Mark all non-serializable fields
      public object[] Parameters { get; }
#pragma warning restore CA2235 // Mark all non-serializable fields

      public MethodCalledException() {
      }

      public MethodCalledException(string message) : base(message) {
      }

      public MethodCalledException(string name, object[] parameters) {
         this.Name = name;
         this.Parameters = parameters;
      }

      public MethodCalledException(string message, Exception innerException) : base(message, innerException) {
      }

      protected MethodCalledException(SerializationInfo info, StreamingContext context) : base(info, context) {
      }
   }
}