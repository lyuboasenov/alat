using System.IO;

namespace Alat.Caching.Services {
   public interface IFileSystemService {
      long GetFileSize(string path);
      long GetDirectorySize(string path);
      void EnsureDirectoryExists(string path);
      Stream GetFileReadStream(string filePath);
      Stream GetFileWriteStream(string filePath);
      void DeleteDirectory(string path);
      void DeleteFile(string filePath);
   }
}
