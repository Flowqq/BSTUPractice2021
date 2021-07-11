using System;

namespace Program.Utils
{
    public class PathUtils
    {
        public static string GetCollectionIndexFilepath(CollectionDefinition collectionDefinition)
        {
            return $"{FileSystemConfig.COLLECTIONS_FILEPATH}{collectionDefinition.Id}/{FileSystemConfig.INDEX_FILENAME}";
        }

        public static string GetCollectionDataFilepath(string collectionId)
        {
            return $"{FileSystemConfig.COLLECTIONS_FILEPATH}{collectionId}/";
        }

        public static string GetCollectionDataFilepathByIndex(IdIndex index)
        {
            if (index.IsLeaf)
            {
                var indexColDef = index.ColDef;
                return $"{GetCollectionDataFilepath(indexColDef.Id)}{index.FileName}{FileSystemConfig.INDEX_FILE_POSTFIX}{FileSystemConfig.FILE_EXTENSION}";
            }
            throw new Exception("This index isn't leaf index tree!");
        }
    }
}