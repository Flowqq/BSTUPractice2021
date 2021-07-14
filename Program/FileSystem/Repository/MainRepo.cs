using System;
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

            IndexRepository = new IndexRepository(colDefs, DataUnitDataSource, DataUnitIndexDataSource);
            CollectionDefinitionRepository = new CollectionDefinitionRepository(CollectionDefDataSource, IndexRepository);
            DataUnitRepository = new DataUnitRepository(DataUnitDataSource, IndexRepository);
        }

        public List<DataUnit> LoadCollectionData(string collectionId, Comparison<DataUnit> sortFunc = null)
        {
            var dataUnits = DataUnitRepository.LoadCollectionData(collectionId);
            dataUnits.Sort(sortFunc ?? throw new ArgumentNullException(nameof(sortFunc)));
            return dataUnits;
        }

        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props, Comparison<DataUnit> sortFunc = null)
        {
            var dataUnits = DataUnitRepository.GetDataUnitsByProps(collectionId, props);
            dataUnits.Sort(sortFunc ?? throw new ArgumentNullException(nameof(sortFunc)));
            return dataUnits;
        }

        public List<DataUnit> GetDataUnitsByPropsAllCollections(List<DataUnitProp> props, Comparison<DataUnit> sortFunc = null)
        {
            var dataUnits = DataUnitRepository.GetDataUnitsByPropsAllCollections(props);
            dataUnits.Sort(sortFunc ?? throw new ArgumentNullException(nameof(sortFunc)));
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
            CollectionDefinitionRepository.SaveCollection(collectionDefinition);
        }

        public void CreateCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefinitionRepository.CreateCollection(collectionDefinition);
        }

        public void DeleteCollection(string collectionId)
        {
            CollectionDefinitionRepository.DeleteCollection(collectionId);
        }
    }
}