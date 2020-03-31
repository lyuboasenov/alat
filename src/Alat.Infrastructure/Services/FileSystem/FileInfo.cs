using Alat.Abstractions.FileSystem;

namespace Alat.Infrastructure.Services.FileSystem {
   public class FileInfo : IFileInfo {
      private readonly global::System.IO.FileInfo _info;
      private readonly IDirectory _directory;

      internal FileInfo(string path, IDirectory directory) {
         _info = new global::System.IO.FileInfo(path ?? throw new global::System.ArgumentNullException(nameof(path)));
         _directory = directory ?? throw new global::System.ArgumentNullException(nameof(directory));
      }

      public bool IsReadOnly { get; set; }
      public bool Exists { get { return _info.Exists; } }
      public string DirectoryName { get { return _info.DirectoryName; } }
      public string Directory { get { return _info.Directory.FullName; } }
      public IDirectoryInfo DirectoryInfo { get { return _directory.GetInfo(_info.FullName); } }
      public long Length { get { return _info.Length; } }
      public string Name { get { return _info.Name; } }
      public string FullName { get { return _info.FullName; } }
      public string Extension { get { return _info.Extension; } }
   }
}
