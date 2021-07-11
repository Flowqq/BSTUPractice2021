using System.Collections.Generic;
using Program.DataPage;
using Program.userInterface;

namespace Program.Controller
{
    public class DataUnitRepository
    {
        public IDataUnitFileInterface DataUnitFileInterface { get; }
        public IndexRepository IndexRepository { get; }

        public DataUnitRepository(IDataUnitFileInterface dataUnitFileInterface, IndexRepository indexRepository)
        {
            DataUnitFileInterface = dataUnitFileInterface;
            IndexRepository = indexRepository;
        }

        public DataUnitsPaginator LoadCollectionData(string collectionId, int pageSize = 10)
        {
            var dataUnits = GetAllCollectionDataUnits(collectionId);
            return new DataUnitsPaginator(pageSize, dataUnits);
        }

        public DataUnit GetDataUnitById(string collectionId, string dataUnitId)
        {
            var filePath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnitId);
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
            var filepath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnit.Id);
            DataUnitFileInterface.SaveDataUnit(filepath, dataUnit);
            IndexRepository.AddDataUnit(collectionId, dataUnit.Id);
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            var filepath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            DataUnitFileInterface.DeleteDataUnit(filepath, dataUnitId);
            IndexRepository.RemoveDataUnit(collectionId, dataUnitId);
        }

        protected List<DataUnit> GetAllCollectionDataUnits(string collectionId)
        {
            var dataUnits = new List<DataUnit>();
            var filePaths = IndexRepository.GetAllIndexesDataFilePaths(collectionId);
            foreach (var filePath in filePaths)
            {
                var indexDataUnits = DataUnitFileInterface.LoadDataUnitsFromFile(filePath);
                dataUnits.AddRange(indexDataUnits);
            }

            return dataUnits;
        }
    }
}