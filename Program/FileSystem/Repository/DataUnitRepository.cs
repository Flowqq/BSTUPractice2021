using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Program.Controller.interfaces;
using Program.FileSystem.Utils;
using Program.userInterface;
using Program.Utils;

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
            var dataUnitSavedAsNew = DataUnitDataSource.SaveDataUnit(filepath, dataUnit);
            if (dataUnitSavedAsNew)
            {
                try
                {
                    IndexRepository.MakeBackupOfIndex(collectionId);
                    var indexToDivide = IndexRepository.AddDataUnit(collectionId, dataUnit.Id);
                    if (indexToDivide != null)
                    {
                        try
                        {
                            DivideIndexByTwo(indexToDivide);
                        }
                        catch
                        {
                            IndexRepository.LoadBackupOfIndex(collectionId);
                            throw;
                        }
                    }
                }
                catch
                {
                    DataUnitDataSource.DeleteDataUnit(filepath, dataUnit.Id);
                    throw;
                }
            }
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            var filepath = IndexRepository.GetDataUnitIndexFilepath(collectionId, dataUnitId);
            var dataUnitToDelete = DataUnitDataSource.LoadDataUnitsFromFile(filepath).Find(unit => unit.Id == dataUnitId);
            var dataUnitDeleted = DataUnitDataSource.DeleteDataUnit(filepath, dataUnitId);
            if (dataUnitDeleted)
            {
                try
                {
                    IndexRepository.MakeBackupOfIndex(collectionId);
                    var indexToUnite = IndexRepository.RemoveDataUnit(collectionId, dataUnitId);
                    if (indexToUnite != null)
                    {
                        try
                        {
                            UniteTwoIndex(indexToUnite);
                        }
                        catch
                        {
                            IndexRepository.LoadBackupOfIndex(collectionId);
                            throw;
                        }
                    }
                }
                catch
                {
                    if (dataUnitToDelete != null)
                    {
                        DataUnitDataSource.SaveDataUnit(filepath, dataUnitToDelete);
                    }

                    throw;
                }
            }
        }

        public void DeleteAllCollectionData(string collectionId)
        {
            var deletedIndex = IndexRepository.RemoveIndex(collectionId);
            var collectionDataFilepath = PathUtils.GetCollectionDataFilepath(collectionId);
            try
            {
                Directory.Delete(collectionDataFilepath, true);
            }
            catch
            {
                IndexRepository.AddIndex(deletedIndex);
                throw;
            }
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
        protected void DivideIndexByTwo(IdIndex indexToDivide)
        {
            var midId = IdUtils.SubIds(indexToDivide.Right.GetRealMaxId(), indexToDivide.Left.GetRealMinId());
            var dataFilepath = indexToDivide.GetFilepath();
            var leftFilepath = indexToDivide.Left.GetFilepath();
            var rightFilepath = indexToDivide.Right.GetFilepath();
            DataUnitDataSource.DivideIndexDataByTwo(dataFilepath, leftFilepath, rightFilepath, midId);
        }

        protected void UniteTwoIndex(IdIndex indexToUnite)
        {
            var dataFilepath = indexToUnite.GetFilepath();
            var dataPostfixIndex = dataFilepath.LastIndexOf(FileSystemConfig.DATA_FILE_POSTFIX, StringComparison.Ordinal);
            var leftFilepath = dataFilepath.Insert(dataPostfixIndex, FileSystemConfig.LEFT_INDEX_POSTFIX);
            var rightFilepath = dataFilepath.Insert(dataPostfixIndex, FileSystemConfig.RIGHT_INDEX_POSTFIX);
            DataUnitDataSource.UniteDataIndex(dataFilepath, leftFilepath, rightFilepath);
        }
    }
}