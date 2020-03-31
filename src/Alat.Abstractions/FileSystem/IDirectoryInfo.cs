namespace Alat.Abstractions.FileSystem {
   public interface IDirectoryInfo {
      string Name { get; }
      bool Exists { get; }
      string Parent { get; }
      string Root { get; }
      IDirectoryInfo ParentInfo { get; }
      IDirectoryInfo RootInfo { get; }
      string FullName { get; }
      string Extension { get; }
   }
}
