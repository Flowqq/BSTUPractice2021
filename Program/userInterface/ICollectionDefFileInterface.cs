using System.Collections.Generic;

namespace Program.userInterface
{
    public interface ICollectionDefFileInterface
    {
        List<CollectionDefinition> LoadCollectionDefinitions();
        void SaveCollectionDefinition(CollectionDefinition collectionDefinition);
        CollectionDefinition LoadCollectionDefinition(string colId);
    }
}