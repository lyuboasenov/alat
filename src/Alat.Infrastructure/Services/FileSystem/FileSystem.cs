using Alat.Abstractions.FileSystem;
using System.Collections.Generic;
using System.IO;

namespace Alat.Infrastructure.Services.FileSystem {
   internal class FileSystem : IDirectory, IFile {
      private static readonly FileSystem _instance = new FileSystem();
      private FileSystem() {

      }

      public static IFile GetFile() {
         return _instance;
      }

      public static IDirectory GetDirectory() {
         return _instance;
      }

      public void Create(string path) {
         Directory.CreateDirectory(path);
      }

      public bool Exists(string path) {
         return Directory.Exists(path);
      }

      public void Delete(string path, bool recursive = false) {
         var files = Directory.GetFiles(path);
         var dirs = Directory.GetDirectories(path);

         foreach (var file in files) {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
         }

         if (recursive) {
            foreach (var dir in dirs) {
               Delete(dir, recursive);
            }
         }

         Directory.Delete(path, recursive);
      }

      bool IFile.Exists(string path) {
         return File.Exists(path);
      }

      public void WriteAllText(string path, string content) {
         IFileInfo fileInfo = ((IFile) this).GetInfo(path);
         if (!fileInfo.DirectoryInfo.Exists) {
            Create(fileInfo.Directory);
         }
         File.WriteAllText(path, content);
      }

      public TextWriter CreateText(string path) {
         IFileInfo fileInfo = ((IFile) this).GetInfo(path);
         if (!fileInfo.DirectoryInfo.Exists) {
            Create(fileInfo.Directory);
         }
         return File.CreateText(path);
      }

      public TextReader GetText(string path) {
         return File.OpenText(path);
      }

      public void CreateDirectory(string path) {
         Directory.CreateDirectory(path);
      }

      public IEnumerable<string> GetFiles(string path) {
         return GetFiles(path, false);
      }

      public IEnumerable<string> GetFiles(string path, bool recursive) {
         return GetFiles(path, "*", recursive);
      }

      public IEnumerable<string> GetFiles(string path, string filter) {
         return GetFiles(path, filter, false);
      }

      public IEnumerable<string> GetFiles(string path, string filter, bool recursive) {
         var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
         return Directory.GetFiles(path, filter, searchOption);
      }

      public TextWriter AppendText(string path) {
         IFileInfo fileInfo = ((IFile)this).GetInfo(path);
         if (!fileInfo.DirectoryInfo.Exists) {
            Create(fileInfo.Directory);
         }
         return new StreamWriter(File.Open(path, FileMode.Append));
      }

      public IDirectoryInfo GetInfo(string path) {
         return new DirectoryInfo(path, this);
      }

      IFileInfo IFile.GetInfo(string path) {
         return new FileInfo(path, this);
      }
   }
}
