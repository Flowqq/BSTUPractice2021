using System;
using System.Collections.Generic;
using System.IO;

namespace Program.userInterface
{
    public class CollectionDefinitionFileRepo : ICollectionDefFileInterface
    {
        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            var fileExists = File.Exists(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            if (fileExists)
            {
                var definitions = new List<CollectionDefinition>();
                using (FileStream fileStream = new FileStream(FileSystemConfig.COLLECTION_DEFS_FILEPATH, FileMode.Open))
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
            FileSystemConfig.CreateDirsForFile(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            Console.Write("Collection definitions file not found!");
            return new List<CollectionDefinition>();
        }
        public void SaveCollectionDefinition(CollectionDefinition collectionDefinition)
        {
            var bytes = collectionDefinition.Serialize();
            var fileExists = File.Exists(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            if (fileExists)
            {
                using (FileStream fileStream = new FileStream(FileSystemConfig.COLLECTION_DEFS_FILEPATH, FileMode.Open))
                {
                    var defsCount = SerializeUtils.ReadNextInt(fileStream);
                    defsCount++;
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.WriteByte(SerializeUtils.IntToByte(defsCount));
                    fileStream.Seek(0, SeekOrigin.End);
                    fileStream.Write(bytes.ToArray(), 0, bytes.Count);
                }
            }
            else
            {
                using (FileStream fileStream = new FileStream(FileSystemConfig.COLLECTION_DEFS_FILEPATH, FileMode.Create))
                {
                    fileStream.WriteByte(SerializeUtils.IntToByte(1));
                    fileStream.Write(bytes.ToArray(), 0, bytes.Count);
                }
            }
            
        }

        public CollectionDefinition LoadCollectionDefinition(string colId)
        {
            var fileExists = File.Exists(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            if (fileExists)
            {
                using (FileStream fileStream = new FileStream(FileSystemConfig.COLLECTION_DEFS_FILEPATH, FileMode.Open))
                {
                    var defsCount = SerializeUtils.ReadNextInt(fileStream);
                    for (int i = 0; i < defsCount; i++)
                    {
                        var definition = CollectionDefinition.Deserialize(fileStream);
                        if(definition.Id == colId)
                        {
                            return definition;
                        }
                    }
                }
            }
            throw new Exception($"No collection definition with id {colId} found!");
        }
    }
}