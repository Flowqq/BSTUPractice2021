using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SaveNewCollection(CollectionDefinition collectionDefinition)
        {
            var newColId = collectionDefinition.Id;
            var newColName = collectionDefinition.Name;
            var definitions = LoadCollectionDefinitions();
            if (definitions.Find(def => def.Id == newColId || def.Name == newColName) == null)
            {
                CollectionDefFileInterface.SaveCollectionDefinition(collectionDefinition);
                IndexController.CreateIndex(collectionDefinition);
            }
            else
            {
                throw new Exception($"Collection with name {newColName} and id {newColId} already exists!");
            }
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
        public void SaveDataUnitsToFile(string collectionId, List<DataUnit> dataUnits)
        {
            foreach (var dataUnit in dataUnits)
            {
                var indexFilepath = IndexController.AddDataUnit(collectionId, dataUnit.Id);
                DataUnitFileInterface.SaveDataUnit(indexFilepath, dataUnit);
            }
        }

        public void SaveDataUnit(string collectionId, DataUnit dataUnit)
        {
            SaveDataUnitsToFile(collectionId, new List<DataUnit>() {dataUnit});
        }

        public void UpdateDataUnit(string collectionId, DataUnit dataUnit)
        {
            var filepath = IndexController.GetDataUnitIndexFilepath(collectionId, dataUnit.Id);
            var dataUnits = DataUnitFileInterface.LoadDataUnitsFromFile(filepath);
            var dataUnitToReplace = dataUnits.FirstOrDefault(unit => unit.Id == dataUnit.Id);
            if (dataUnitToReplace != null)
            {
                dataUnits.Remove(dataUnitToReplace);
            }
            dataUnits.Add(dataUnit);
            DataUnitFileInterface.DeleteFile(filepath);
            SaveDataUnitsToFile(filepath, dataUnits);
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            var filepath = IndexController.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            var dataUnits = DataUnitFileInterface.LoadDataUnitsFromFile(filepath);
            var dataUnitToDelete = dataUnits.FirstOrDefault(unit => unit.Id == dataUnitId);
            if (dataUnitToDelete != null)
            {
                dataUnits.Remove(dataUnitToDelete);
            }
            DataUnitFileInterface.DeleteFile(filepath);
            SaveDataUnitsToFile(filepath, dataUnits);
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