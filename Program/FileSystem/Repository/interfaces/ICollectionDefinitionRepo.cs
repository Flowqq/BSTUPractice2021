using System.Collections.Generic;

namespace Program.Controller.interfaces
{
    public interface ICollectionDefinitionRepo
    {
        public List<CollectionDefinition> LoadCollectionDefinitions();
        public void SaveCollection(CollectionDefinition collectionDefinition);
        public void CreateCollection(CollectionDefinition collectionDefinition);
        public void DeleteCollection(string collectionId);
    }
}