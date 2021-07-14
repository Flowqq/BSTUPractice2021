using System.Collections.Generic;
using System.Linq;
using Program.Controller.interfaces;
using Program.userInterface;

namespace Program.Controller
{
    public class DataUnitRepository : IDataUnitRepo
    {
        public IDataUnitDataSource DataUnitDataSource { get; }
        public IndexRepository IndexRepository { get; }

        public DataUnitRepository(IDataUnitDataSource dataUnitDataSource, IndexRepository indexRepository)
        {
            DataUnitDataSource = dataUnitDataSource;
            IndexRepository = indexRepository;
        }

        public List<DataUnit> LoadCollectionData(string collectionId)
        {
            return GetAllCollectionDataUnits(collectionId);
        }

        public List<DataUnit> GetDataUnitsByPropsAllCollections(List<DataUnitProp> props)
        {
            var resultList = new List<DataUnit>();
            foreach (var index in IndexRepository.AllIndexes)
            {
                resultList.AddRange(GetDataUnitsByProps(index.CollectionId, props));
            }
            return resultList;
        }

        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props)
        {
            var dataUnits = new List<DataUnit>();
            var filePaths = IndexRepository.GetAllIndexesDataFilePaths(collectionId);
            foreach (var filePath in filePaths)
            {
                var indexDataUnits = DataUnitDataSource.LoadDataUnitsFromFile(filePath);
                indexDataUnits = SearchDataUnits(indexDataUnits, props);
                dataUnits.AddRange(indexDataUnits);
            }
            return dataUnits;
        }

        public void SaveDataUnit(string collectionId, DataUnit dataUnit)
        {
            var filepath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnit.Id);
            DataUnitDataSource.SaveDataUnit(filepath, dataUnit);
            IndexRepository.AddDataUnit(collectionId, dataUnit.Id);
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            var filepath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            DataUnitDataSource.DeleteDataUnit(filepath, dataUnitId);
            IndexRepository.RemoveDataUnit(collectionId, dataUnitId);
        }
        protected List<DataUnit> GetAllCollectionDataUnits(string collectionId)
        {
            var dataUnits = new List<DataUnit>();
            var filePaths = IndexRepository.GetAllIndexesDataFilePaths(collectionId);
            foreach (var filePath in filePaths)
            {
                var indexDataUnits = DataUnitDataSource.LoadDataUnitsFromFile(filePath);
                dataUnits.AddRange(indexDataUnits);
            }
            return dataUnits;
        }
        protected List<DataUnit> SearchDataUnits(List<DataUnit> dataUnits, List<DataUnitProp> searchProps)
        {
            var resultSet = new List<DataUnit>();
            foreach (var dataUnit in dataUnits)
            {
                var matches = searchProps.All(searchField => dataUnit.GetProperty(searchField.Name).Value.Equals(searchField.Value));
                if (matches)
                {
                    resultSet.Add(dataUnit);
                }
            }
            return resultSet;
        }
    }
}