using System.Collections.Generic;
using System.IO;
using Program.Controller;
using Program.Controller.interfaces;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class MainRepo : IMainRepo
    {
        protected ICollectionDefDataSource CollectionDefDataSource;
        protected IDataUnitDataSource DataUnitDataSource;
        protected IDataUnitIndexDataSource DataUnitIndexDataSource;
        protected IndexRepository IndexRepository;
        protected  CollectionDefinitionRepository CollectionDefinitionRepository { get; }
        protected  DataUnitRepository DataUnitRepository { get; }

        public MainRepo()
        {
            Directory.CreateDirectory(FileSystemConfig.COLLECTIONS_DATA_FILEPATH);
            DirUtils.CreateDirsForFile(FileSystemConfig.COLLECTION_DEFS_FILEPATH);
            
            DataUnitDataSource = new DataUnitDataSource();
            DataUnitIndexDataSource = new DataUnitIndexDataSource();
            CollectionDefDataSource = new CollectionDefinitionDataSource();

            var colDefs = CollectionDefDataSource.LoadCollectionDefinitions();

            IndexRepository = new IndexRepository(colDefs, DataUnitIndexDataSource);
            CollectionDefinitionRepository = new CollectionDefinitionRepository(CollectionDefDataSource);
            DataUnitRepository = new DataUnitRepository(DataUnitDataSource, IndexRepository);
        }

        public List<DataUnit> LoadCollectionData(string collectionId)
        {
            return DataUnitRepository.LoadCollectionData(collectionId);
        }

        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props)
        {
            return DataUnitRepository.GetDataUnitsByProps(collectionId, props);
        }

        public List<DataUnit> GetDataUnitsByPropsAllCollections(List<DataUnitProp> props)
        {
            var dataUnits = DataUnitRepository.GetDataUnitsByPropsAllCollections(props);
            return dataUnits;
        }

        public void SaveDataUnit(string collectionId, DataUnit dataUnit)
        {
            DataUnitRepository.SaveDataUnit(collectionId, dataUnit);
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            DataUnitRepository.DeleteDataUnit(collectionId, dataUnitId);
        }

        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            return CollectionDefinitionRepository.LoadCollectionDefinitions();
        }

        public void SaveCollection(CollectionDefinition collectionDefinition)
        {
            var oldColDef = CollectionDefinitionRepository.SaveCollection(collectionDefinition);
            try
            {
                IndexRepository.CreateIndex(collectionDefinition.Id);
            }
            catch
            {
                if (oldColDef != null)
                {
                    CollectionDefinitionRepository.SaveCollection(oldColDef);
                }
                else
                {
                    CollectionDefinitionRepository.DeleteCollection(collectionDefinition.Id);
                }
                throw;
            }
        }

        public void CreateCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefinitionRepository.CreateCollection(collectionDefinition);
            try
            {
                IndexRepository.CreateIndex(collectionDefinition.Id);
            }
            catch
            {
                CollectionDefDataSource.DeleteCollection(collectionDefinition.Id);
                throw;
            }
        }

        public void DeleteCollection(string collectionId)
        {
            var deletedCollection = CollectionDefinitionRepository.DeleteCollection(collectionId);
            try
            {
                DataUnitRepository.DeleteAllCollectionData(collectionId);
            }
            catch
            {
                CollectionDefinitionRepository.CreateCollection(deletedCollection);
                throw;
            }
        }
    }
}