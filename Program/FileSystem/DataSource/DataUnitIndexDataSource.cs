using System;
using System.Collections.Generic;
using System.IO;
using Program.userInterface;

namespace Program
{
    public class IndexFileInterface : IIndexFileInterface
    {
        public void SaveIndexToFile(IdIndex index)
        {
            var indexFilepath = FileSystemConfig.GetCollectionIndexFilepath(index.ColDef);
            var dirFilepath = FileSystemConfig.GetDirPathFomFilePath(indexFilepath);
            if (!File.Exists(indexFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
            using (FileStream fileStream = new FileStream(indexFilepath, FileMode.Create))
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
                var indexFilepath = FileSystemConfig.GetCollectionIndexFilepath(colDef);
                var fileExists = File.Exists(indexFilepath);
                if (fileExists)
                {
                    using (FileStream fileStream = new FileStream(indexFilepath, FileMode.Open))
                    {
                        var index = IdIndex.Deserialize(fileStream, colDef);
                        collectionsIndexes.Add(index);
                    }
                }
                else
                {
                    throw new Exception($"Index for collection with name {colDef.Name} not found!");
                }
            }
            return collectionsIndexes;        
        }

        public void CreateIndex(CollectionDefinition collectionDefinition)
        {
            var dirFilepath = FileSystemConfig.GetCollectionIndexFilepath(collectionDefinition);
            if (Directory.Exists(dirFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
        }
    }
}