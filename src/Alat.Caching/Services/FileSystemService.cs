using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Alat.Caching.Services {
   public interface FileSystemService {
      long GetFileSize(string path);
      long GetDirectorySize(string path);
      void EnsureDirectoryExists(string path);
      Stream GetFileReadStream(string filePath);
      Stream GetFileWriteStream(string filePath);
      Stream GetFileCreateStream(string filePath);
      void DeleteDirectory(string path);
      void DeleteFile(string filePath);
   }
}
