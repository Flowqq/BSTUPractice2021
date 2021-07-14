using System.Collections.Generic;
using Program.Controller.interfaces;
using Program.userInterface;

namespace Program.Controller
{
    public class CollectionDefinitionRepository : ICollectionDefinitionRepo
    {
        public ICollectionDefDataSource CollectionDefDataSource { get; }

        public CollectionDefinitionRepository(ICollectionDefDataSource collectionDefDataSource)
        {
            CollectionDefDataSource = collectionDefDataSource;
        }

        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            return CollectionDefDataSource.LoadCollectionDefinitions();
        }

        public CollectionDefinition SaveCollection(CollectionDefinition collectionDefinition)
        {
            var oldDef = CollectionDefDataSource.LoadCollectionDefinitions()
                .Find(def => def.Id == collectionDefinition.Id);
            CollectionDefDataSource.SaveCollectionDefinition(collectionDefinition);
            return oldDef;
        }
        public void CreateCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefDataSource.SaveCollectionDefinition(collectionDefinition);
        }

        public CollectionDefinition DeleteCollection(string collectionId)
        {
            return CollectionDefDataSource.DeleteCollection(collectionId);
        }
    }
}