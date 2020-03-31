using System.Collections.Generic;

namespace Alat.Abstractions.FileSystem {
   public interface IDirectory {
      void Create(string path);
      void Delete(string path, bool recursive = false);
      bool Exists(string path);
      void CreateDirectory(string path);
      IEnumerable<string> GetFiles(string path);
      IEnumerable<string> GetFiles(string path, bool recursive);
      IEnumerable<string> GetFiles(string path, string filter);
      IEnumerable<string> GetFiles(string path, string filter, bool recursive);
      IDirectoryInfo GetInfo(string path);
   }
}