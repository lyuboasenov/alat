using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Testing.Mocks {
   public class NotifyingMockObserver : IObserver<MethodCall> {

      private IList<MethodCall> calledMethods = new List<MethodCall>();

      public IEnumerable<MethodCall> CalledMethods { get { return calledMethods; } }

      public void OnCompleted() { }

      public void OnError(Exception error) { }

      public void OnNext(MethodCall value) {
         calledMethods.Add(value);
      }

      public bool IsMethodCalledIgnoreParameters(string name) {
         return calledMethods.Any(cm => cm.MethodName == name);
      }

      public bool IsMethodCalled(string name) {
         return IsMethodCalled(new MethodCall(name));
      }

      public bool IsMethodCalled(string name, params object[] parameters) {
         return IsMethodCalled(new MethodCall(name, parameters));
      }

      public bool IsMethodCalled(MethodCall methodCall) {
         return calledMethods.Any(mc => mc.MethodName == methodCall.MethodName &&
            (mc.Parameters == null && methodCall.Parameters == null ||
            mc.Parameters.SequenceEqual(methodCall.Parameters)));
      }
   }
}
