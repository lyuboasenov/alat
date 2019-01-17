using System.IO;
using System.Linq;

namespace Alat.Caching.Services.Impl {
   public class FileSystemService : Services.FileSystemService {
      public void DeleteDirectory(string path) {
         Directory.Delete(path, true);
      }

      public void DeleteFile(string filePath) {
         File.Delete(filePath);
      }

      public void EnsureDirectoryExists(string path) {
         if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
         }
      }

      public long GetDirectorySize(string path) {
         return Directory.GetFiles(path, "*", SearchOption.AllDirectories).Sum(t => (new FileInfo(t).Length));
      }

      public Stream GetFileCreateStream(string filePath) {
         return File.Open(filePath, FileMode.Create, FileAccess.Read);
      }

      public Stream GetFileReadStream(string filePath) {
         return File.Open(filePath, FileMode.Open, FileAccess.Read);
      }

      public long GetFileSize(string filePath) {
         var fileInfo = new FileInfo(filePath);
         return fileInfo.Length;
      }

      public Stream GetFileWriteStream(string filePath) {
         return File.Open(filePath, FileMode.Open, FileAccess.Write);
      }
   }
}
