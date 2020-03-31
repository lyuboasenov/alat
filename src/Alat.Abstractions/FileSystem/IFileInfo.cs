namespace Alat.Abstractions.FileSystem {
   public interface IFileInfo {
      bool IsReadOnly { get; set; }
      bool Exists { get; }
      string DirectoryName { get; }
      string Directory { get; }
      IDirectoryInfo DirectoryInfo { get; }
      long Length { get; }
      string Name { get; }
      string FullName { get; }
      string Extension { get; }

   }
}
