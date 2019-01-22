using Alat.Http.Caching.Sessions;
using System;
using Xunit;

namespace Alat.Http.Tests.SessionStack {
   public class SessionStackTests {
      [Fact]
      public void AddKeyInEmptyStack() {
         var key = "dummy";
         var sessionStack = new Caching.Sessions.SessionStack();
         sessionStack.AddKeyToTopFrame(key);
      }

      [Fact]
      public void AddKey() {
         var key = "dummy";

         var sessionStack = new Caching.Sessions.SessionStack();

         var frame1 = GetFrame();
         var frame2 = GetFrame();

         sessionStack.Push(frame1);
         sessionStack.Push(frame2);

         sessionStack.AddKeyToTopFrame(key);

         Assert.Contains(key, frame2.Keys);
         Assert.DoesNotContain(key, frame1.Keys);
      }

      [Fact]
      public void GetCachingPeriodFromEmpty() {
         var sessionStack = new Caching.Sessions.SessionStack();

         Assert.Throws<ArgumentException>(() => sessionStack.GetCachingPeriod());
      }

      [Fact]
      public void GetCachingPeriod() {
         var cachingPeriodInMinutes = 3;

         var sessionStack = new Caching.Sessions.SessionStack();
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame(cachingPeriodInMinutes));

         Assert.Equal(TimeSpan.FromMinutes(cachingPeriodInMinutes), sessionStack.GetCachingPeriod());
      }

      [Fact]
      public void PushPop() {
         var sessionStack = new Caching.Sessions.SessionStack();
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         var frame = GetFrame();
         sessionStack.Push(frame);

         Assert.Equal(frame, sessionStack.Pop(frame.SessionId));
         Assert.False(sessionStack.IsEmpty());
      }

      [Fact]
      public void PushPopNotLastSession() {
         var sessionStack = new Caching.Sessions.SessionStack();
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());

         var frame = GetFrame();
         sessionStack.Push(frame);

         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());

         Assert.Throws<ArgumentException>(() => sessionStack.Pop(frame.SessionId));
      }

      [Fact]
      public void PushPopNotExisting() {
         var sessionStack = new Caching.Sessions.SessionStack();
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());
         sessionStack.Push(GetFrame());

         Assert.Throws<ArgumentException>(() => sessionStack.Pop(Guid.NewGuid()));
         Assert.False(sessionStack.IsEmpty());
      }

      [Fact]
      public void PushNull() {
         var sessionStack = new Caching.Sessions.SessionStack();

         Assert.Throws<ArgumentNullException>(() => sessionStack.Push(null));
      }



      private SessionFrame GetFrame(int cachingPeriodInMinutes = 5) {
         return new SessionFrame(Guid.NewGuid(), TimeSpan.FromMinutes(cachingPeriodInMinutes));
      }
   }
}
