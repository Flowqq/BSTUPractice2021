using System.Collections.Generic;
using Program.DataPage;
using Program.userInterface;

namespace Program.Controller
{
    public class FileSystemController
    {
        public ICollectionDefFileInterface CollectionDefFileInterface { get; }
        public IDataUnitFileInterface DataUnitFileInterface { get; }
        public IndexController IndexController { get; }

        public FileSystemController(ICollectionDefFileInterface collectionDefFileInterface,
            IDataUnitFileInterface dataUnitFileInterface, IndexController indexController)
        {
            CollectionDefFileInterface = collectionDefFileInterface;
            DataUnitFileInterface = dataUnitFileInterface;
            IndexController = indexController;
        }

        public List<CollectionDefinition> LoadCollectionDefinitions()
        {
            return CollectionDefFileInterface.LoadCollectionDefinitions();
        }

        public void SaveCollection(CollectionDefinition collectionDefinition)
        {
            CollectionDefFileInterface.SaveCollectionDefinition(collectionDefinition);
            IndexController.CreateIndex(collectionDefinition);
        }

        public void DeleteCollection(string collectionId)
        {
            CollectionDefFileInterface.DeleteCollection(collectionId);
        }

        public DataUnitsPaginator LoadCollectionData(string collectionId, int pageSize = 10)
        {
            var dataUnits = GetAllCollectionDataUnits(collectionId);
            return new DataUnitsPaginator(pageSize, dataUnits);
        }

        public DataUnit GetDataUnitById(string collectionId, string dataUnitId)
        {
            var filePath = IndexController.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            var indexDataUnits = DataUnitFileInterface.LoadDataUnitsFromFile(filePath);
            return indexDataUnits.Find(unit => unit.Id == dataUnitId);
        }

        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props)
        {
            var dataUnits = GetAllCollectionDataUnits(collectionId);
            var collection = new Collection("", "", dataUnits);
            return collection.SearchDataUnits(props);
        }

        public void SaveDataUnit(string collectionId, DataUnit dataUnit)
        {
            var filepath = IndexController.GetDataUnitIndexFilepath(collectionId, dataUnit.Id);
            DataUnitFileInterface.SaveDataUnit(filepath, dataUnit);
            IndexController.AddDataUnit(collectionId, dataUnit.Id);
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            var filepath = IndexController.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            DataUnitFileInterface.DeleteDataUnit(filepath, dataUnitId);
            IndexController.RemoveDataUnit(collectionId, dataUnitId);
        }

        protected List<DataUnit> GetAllCollectionDataUnits(string collectionId)
        {
            var dataUnits = new List<DataUnit>();
            var filePaths = IndexController.GetAllIndexesDataFilePaths(collectionId);
            foreach (var filePath in filePaths)
            {
                var indexDataUnits = DataUnitFileInterface.LoadDataUnitsFromFile(filePath);
                dataUnits.AddRange(indexDataUnits);
            }

            return dataUnits;
        }
    }
}