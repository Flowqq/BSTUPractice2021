using System;
using System.IO;

namespace Program.Utils
{
    public class DirUtils
    {
        public static string GetDirPathFomFilePath(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf('/'));
        }
        public static void CreateDirsForFile(string filepath)
        {
            var dirFilepath = GetDirPathFomFilePath(filepath);
            if (!File.Exists(dirFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
        }

        public static void CreateFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                CreateDirsForFile(filepath);
                File.Create(filepath).Dispose();
            }
            else
            {
                throw new Exception($"File {filepath} already exists!");
            }
        }
        public static void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            else
            {
                throw new Exception($"File to delete - {filepath} not exists!");
            }
        }
    }
}