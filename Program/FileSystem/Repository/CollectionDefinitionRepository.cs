using System.Collections.Generic;
using Program.userInterface;

namespace Program.Controller
{
    public class CollectionDefinitionRepository
    {
        public ICollectionDefFileInterface CollectionDefFileInterface { get; }
        public IndexRepository IndexRepository { get; }

        public CollectionDefinitionRepository(ICollectionDefFileInterface collectionDefFileInterface, IndexRepository indexRepository)
        {
            CollectionDefFileInterface = collectionDefFileInterface;
            IndexRepository = indexRepository;
        }

        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            return CollectionDefFileInterface.LoadCollectionDefinitions();
        }

        public void SaveCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefFileInterface.SaveCollectionDefinition(collectionDefinition);
            IndexRepository.CreateIndex(collectionDefinition);
        }

        public void DeleteCollection(string collectionId)
        {
            CollectionDefFileInterface.DeleteCollection(collectionId);
            IndexRepository.RemoveIndex(collectionId);
        }
    }
}