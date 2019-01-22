using System;
using System.Collections.Generic;
using System.Linq;

namespace Alat.Http.Caching.Sessions {
   internal class SessionStack {
      private readonly object lockObj = new object();

      private Stack<SessionFrame> Stack { get; }

      public SessionStack() {
         Stack = new Stack<SessionFrame>();
      }

      public void AddKeyToTopFrame(string key) {
         lock (lockObj) {
            if (!IsEmpty()) {
               Stack.Peek().Keys.Add(key);
            }
         }
      }

      public TimeSpan GetCachingPeriod() {
         lock (lockObj) {
            if (Stack.Count == 0) {
               throw new ArgumentException("Stack is empty");
            }

            return Stack.Peek().CachingPeriod;
         }
      }

      public void Push(SessionFrame sessionFrame) {
         if (null == sessionFrame) {
            throw new ArgumentNullException(nameof(sessionFrame));
         }

         lock (lockObj) {
            Stack.Push(sessionFrame);
         }
      }

      public bool IsEmpty() {
         return !Stack.Any();
      }

      public SessionFrame Pop(Guid sessionId) {
         lock (lockObj) {
            if (Stack.Peek().SessionId != sessionId) {
               throw new ArgumentException("Session not found on the top of the stack");
            }

            return Stack.Pop();
         }
      }
   }
}
