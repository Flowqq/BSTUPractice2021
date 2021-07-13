using System;
using System.Collections.Generic;
using System.IO;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class DataUnitIndexDataSource : IDataUnitIndexDataSource
    {
        public void SaveIndexToFile(IdIndex index)
        {
            var indexFilepath = PathUtils.GetCollectionIndexFilepath(index.CollectionId);
            var dirFilepath = DirUtils.GetDirPathFomFilePath(indexFilepath);
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
                var indexFilepath = PathUtils.GetCollectionIndexFilepath(colDef.Id);
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
            var dirFilepath = PathUtils.GetCollectionIndexFilepath(collectionDefinition.Id);
            if (Directory.Exists(dirFilepath))
            {
                Directory.CreateDirectory(dirFilepath);
            }
        }
    }
}