using System.Collections.Generic;
using System.Linq;
using Program.Controller.interfaces;
using Program.userInterface;
using Program.Utils;

namespace Program.Controller
{
    public class CollectionDefinitionRepository : ICollectionDefinitionRepo
    {
        public ICollectionDefDataSource CollectionDefDataSource { get; }
        public IndexRepository IndexRepository { get; }

        public CollectionDefinitionRepository(ICollectionDefDataSource collectionDefDataSource, IndexRepository indexRepository)
        {
            CollectionDefDataSource = collectionDefDataSource;
            IndexRepository = indexRepository;
        }

        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            return CollectionDefDataSource.LoadCollectionDefinitions();
        }

        public void SaveCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefDataSource.SaveCollectionDefinition(collectionDefinition);
            IndexRepository.CreateIndex(collectionDefinition);
        }
        public void CreateCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefDataSource.SaveCollectionDefinition(collectionDefinition);
            IndexRepository.CreateIndex(collectionDefinition);
            var firstDataFilePath = IndexRepository.GetAllIndexesDataFilePaths(collectionDefinition.Id).FirstOrDefault();
            DirUtils.CreateFile(firstDataFilePath);
        }

        public void DeleteCollection(string collectionId)
        {
            CollectionDefDataSource.DeleteCollection(collectionId);
            IndexRepository.RemoveIndex(collectionId);
        }
    }
}