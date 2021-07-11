using System.Collections.Generic;

namespace Program.userInterface
{
    public interface ICollectionDefFileInterface
    {
        List<CollectionDefinition> LoadCollectionDefinitions();
        void SaveCollectionDefinition(CollectionDefinition collectionDefinition);
        public void DeleteCollection(string collectionId);
    }
}