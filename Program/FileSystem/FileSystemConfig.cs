using System;
using System.IO;

namespace Program
{
    public class FileSystemConfig
    {
        public static readonly string FILE_EXTENSION = ".dat";
        public static readonly string COLLECTION_DEFS_FILEPATH = "db/colsDefs" + FILE_EXTENSION;
        public static readonly string COLLECTIONS_FILEPATH = "db/collections/";

        public static string GetCollectionIndexFilepath(CollectionDefinition collectionDefinition)
        {
            return $"{COLLECTIONS_FILEPATH}{collectionDefinition.Id}/colIndex{FILE_EXTENSION}";
        }

        public static string GetCollectionDataFilepath(string collectionId)
        {
            return $"{COLLECTIONS_FILEPATH}{collectionId}/";
        }

        public static string GetCollectionDataFilepathByIndex(IdIndex index)
        {
            if (index.IsLeaf)
            {
                var indexColDef = index.ColDef;
                return $"{GetCollectionDataFilepath(indexColDef.Id)}{index.FileName}Index{FILE_EXTENSION}";
            }
            throw new Exception("This index isn't leaf index tree!");
        }

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