using System.Collections.Generic;

namespace Program.Controller.interfaces
{
    public interface ICollectionDefinitionRepo
    {
        public List<CollectionDefinition> LoadCollectionDefinitions();
        public CollectionDefinition SaveCollection(CollectionDefinition collectionDefinition);
        public void CreateCollection(CollectionDefinition collectionDefinition);
        public CollectionDefinition DeleteCollection(string collectionId);
    }
}