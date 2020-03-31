using Alat.Abstractions.FileSystem;

namespace Alat.Infrastructure.Services.FileSystem {
   public class DirectoryInfo : IDirectoryInfo {
      private readonly global::System.IO.DirectoryInfo _info;
      private readonly IDirectory _directory;

      internal DirectoryInfo(string path, IDirectory directory) {
         _info = new global::System.IO.DirectoryInfo(path ?? throw new global::System.ArgumentNullException(nameof(path)));
         _directory = directory ?? throw new global::System.ArgumentNullException(nameof(directory));
      }

      public string Name { get { return _info.Name; } }
      public bool Exists { get { return _info.Exists; } }
      public string Parent { get { return _info.Parent?.FullName; } }
      public string Root { get { return _info.Root?.FullName; } }
      public IDirectoryInfo ParentInfo { get { return _directory.GetInfo(Parent); } }
      public IDirectoryInfo RootInfo { get { return _directory.GetInfo(Root); } }
      public string FullName { get { return _info.FullName; } }
      public string Extension { get { return _info.Extension; } }
   }
}
