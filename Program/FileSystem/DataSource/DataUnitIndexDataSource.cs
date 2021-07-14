using System.Collections.Generic;
using System.IO;
using Program.Exceptions;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class DataUnitIndexDataSource : IDataUnitIndexDataSource
    {
        public void UpdateIndexFile(IdIndex index)
        {
            var indexFilepath = PathUtils.GetCollectionIndexFilepath(index.CollectionId);
            var dirFilepath = DirUtils.GetDirPathFomFilePath(indexFilepath);
            if (!File.Exists(indexFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
            using (var fileStream = new FileStream(indexFilepath, FileMode.Create))
            {
                var indexerBytes = index.Serialize();
                fileStream.Write(indexerBytes.ToArray(), 0, indexerBytes.Count);
            }
        }

        public List<IdIndex> LoadIndexes(List<CollectionDefinition> colDefs)
        {
            var collectionsIndexes = new List<IdIndex>();
            foreach (var colDef in colDefs)
            {
                var indexFilepath = PathUtils.GetCollectionIndexFilepath(colDef.Id);
                var fileExists = File.Exists(indexFilepath);
                if (fileExists)
                {
                    using (var fileStream = new FileStream(indexFilepath, FileMode.Open))
                    {
                        var index = IdIndex.Deserialize(fileStream, colDef.Id);
                        collectionsIndexes.Add(index);
                    }
                }
                else
                {
                    throw IndexNotFoundException.GenerateException(colDef.Id);
                }
            }
            return collectionsIndexes;        
        }

        public void CreateIndex(string collectionId)
        {
            var dirFilepath = PathUtils.GetCollectionIndexFilepath(collectionId);
            if (Directory.Exists(dirFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
        }

        public void SaveIndexToFile(string filepath, IdIndex index)
        {
            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                var indexerBytes = index.Serialize();
                fileStream.Write(indexerBytes.ToArray(), 0, indexerBytes.Count);
            }
        }

        public IdIndex LoadIndexFromFile(string filepath, string collectionId)
        {
            var fileExists = File.Exists(filepath);
            if (fileExists)
            {
                using (var fileStream = new FileStream(filepath, FileMode.Open))
                {
                    return IdIndex.Deserialize(fileStream, collectionId);
                }
            }
            throw new FileNotFoundException($"File for load index {filepath} not found!");
        }
    }
}