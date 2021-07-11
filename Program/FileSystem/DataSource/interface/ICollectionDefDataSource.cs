using System.Collections.Generic;

namespace Program.userInterface
{
    public interface ICollectionDefDataSource
    {
        List<CollectionDefinition> LoadCollectionDefinitions();
        void SaveCollectionDefinition(CollectionDefinition collectionDefinition);
        public void DeleteCollection(string collectionId);
    }
}