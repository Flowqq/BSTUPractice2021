using System;
using System.Collections.Generic;
using System.IO;

namespace Program.userInterface
{
    public class CollectionDefinitionFileRepo : ICollectionDefFileInterface
    {
        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            var filepath = FileSystemConfig.COLLECTION_DEFS_FILEPATH;
            var fileExists = File.Exists(filepath);
            if (fileExists)
            {
                var definitions = new List<CollectionDefinition>();
                using (FileStream fileStream = new FileStream(filepath, FileMode.Open))
                {
                    var defsCount = SerializeUtils.ReadNextInt(fileStream);
                    for (int i = 0; i < defsCount; i++)
                    {
                        var definition = CollectionDefinition.Deserialize(fileStream);
                        definitions.Add(definition);
                    }
                }
                return definitions;
            }
            throw new Exception($"File with collections definitions by path {filepath} not found!");
        }

        public void SaveCollectionDefinition(CollectionDefinition collectionDefinition)
        {
            var definitions = LoadCollectionDefinitions();
            var collectionToSave = definitions.Find(def => def.Id == collectionDefinition.Id);
            if (collectionToSave == null)
            {
                definitions.Add(collectionDefinition);
            }
            else
            {
                definitions.Remove(collectionToSave);
                definitions.Add(collectionDefinition);
            }
            RewriteCollectionDefinitions(definitions);
        }

        public void DeleteCollection(string collectionId)
        {
            var definitions = LoadCollectionDefinitions();
            var collectionToDelete = definitions.Find(def => def.Id == collectionId);
            if (collectionToDelete != null)
            {
                definitions.Remove(collectionToDelete);
                RewriteCollectionDefinitions(definitions);
            }
            else
            {
                throw new Exception($"Collection to delete with id {collectionId} not found!");
            }
        }

        protected void RewriteCollectionDefinitions(List<CollectionDefinition> collectionDefinitions)
        {
            var fileExists = File.Exists(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            if (!fileExists)
            {
                FileSystemConfig.CreateDirsForFile(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            }
            using (FileStream fileStream = new FileStream(FileSystemConfig.COLLECTION_DEFS_FILEPATH, FileMode.Create))
            {
                var bytes = new List<byte>();
                var defsCount = collectionDefinitions.Count;
                bytes.Add(SerializeUtils.IntToByte(defsCount));
                foreach (var colDef in collectionDefinitions)
                {
                    bytes.AddRange(colDef.Serialize());
                }
                fileStream.Write(bytes.ToArray(), 0, bytes.Count);
            }
        }
    }
}