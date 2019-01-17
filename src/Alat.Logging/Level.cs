using System;

namespace Alat.Logging {
   public class Level : IComparable {

      public static Level Off { get; } = new Level(int.MaxValue, nameof(Off));
      public static Level Fatal { get; } = new Level(25000, nameof(Fatal));
      public static Level Error { get; } = new Level(20000, nameof(Error));
      public static Level Warn { get; } = new Level(15000, nameof(Warn));
      public static Level Info { get; } = new Level(10000, nameof(Info));
      public static Level Debug { get; } = new Level(5000, nameof(Debug));
      public static Level Trace { get; } = new Level(1000, nameof(Trace));
      public static Level All { get; } = new Level(int.MinValue, nameof(All));

      private int Severity { get; }
      public string Name { get; }

      public Level(int severity, string name) {
         Severity = severity;
         Name = name;
      }

      public override bool Equals(object o) {
         Level otherLevel = o as Level;
         if (otherLevel != null) {
            return Severity == otherLevel.Severity;
         } else {
            return base.Equals(o);
         }
      }

      public override int GetHashCode() {
         return Severity;
      }

      public override string ToString() {
         return Name;
      }

      public int CompareTo(object obj) {
         Level target = obj as Level;
         if (target != null) {
            return Compare(this, target);
         }
         throw new ArgumentException($"{nameof(obj)} [{obj}] is not an instance of Level");
      }

      public static bool operator >(Level l, Level r) {
         return l.Severity > r.Severity;
      }

      public static bool operator <(Level l, Level r) {
         return l.Severity < r.Severity;
      }

      public static bool operator >=(Level l, Level r) {
         return l.Severity >= r.Severity;
      }

      public static bool operator <=(Level l, Level r) {
         return l.Severity <= r.Severity;
      }

      public static bool operator ==(Level l, Level r) {
         if (((object)l) != null && ((object)r) != null) {
            return l.Severity == r.Severity;
         } else {
            return ((object)l) == ((object)r);
         }
      }

      public static bool operator !=(Level l, Level r) {
         return !(l == r);
      }

      public static int Compare(Level l, Level r) {
         // Reference equals
         if ((object)l == (object)r) {
            return 0;
         }

         if (l == null && r == null) {
            return 0;
         }
         if (l == null) {
            return -1;
         }
         if (r == null) {
            return 1;
         }

         return l.Severity.CompareTo(r.Severity);
      }
   }
}
