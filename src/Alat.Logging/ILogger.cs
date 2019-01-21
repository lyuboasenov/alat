namespace Alat.Logging {
   public interface ILogger {
      void Log(object obj);
      void Log(object obj, Level level);

      void Log(string message);
      void Log(string message, Level level);

      void Debug(string message);
      void Info(string message);
      void Warn(string message);
      void Error(string message);
      void Fatal(string message);

      void Debug(object obj);
      void Info(object obj);
      void Warn(object obj);
      void Error(object obj);
      void Fatal(object obj);
   }
}