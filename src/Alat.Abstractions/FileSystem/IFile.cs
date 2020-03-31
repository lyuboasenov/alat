using System.IO;

namespace Alat.Abstractions.FileSystem {
   public interface IFile {
      bool Exists(string path);
      void WriteAllText(string path, string content);
      TextWriter CreateText(string path);
      TextWriter AppendText(string path);
      TextReader GetText(string path);
      IFileInfo GetInfo(string path);
   }
}
