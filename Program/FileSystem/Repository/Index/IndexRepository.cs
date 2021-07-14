using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Program.userInterface;

namespace Program
{
    public class IndexRepository
    {
        protected IDataUnitDataSource DataUnitDataSource { get; }
        protected  IDataUnitIndexDataSource DataUnitIndexDataSource { get; }
        public List<IdIndex> AllIndexes { get; }

        public IndexRepository(List<CollectionDefinition> colDefs, IDataUnitDataSource dataUnitDataSource,
            IDataUnitIndexDataSource dataUnitIndexDataSource)
        {
            DataUnitDataSource = dataUnitDataSource;
            DataUnitIndexDataSource = dataUnitIndexDataSource;
            AllIndexes = DataUnitIndexDataSource.LoadIndexes(colDefs);
        }

        public void CreateIndex(CollectionDefinition collectionDefinition)
        {
            var index = FindIndexByCollectionId(collectionDefinition.Id);
            if (index == null)
            {
                var newIndex = new IdIndex(collectionDefinition);
                DataUnitIndexDataSource.CreateIndex(collectionDefinition);
                DataUnitIndexDataSource.SaveIndexToFile(newIndex);
                AllIndexes.Add(newIndex);
            }
        }

        public void RemoveIndex(string collectionId)
        {
            AllIndexes.RemoveAll(index => index.CollectionId == collectionId);
        }

        public string AddDataUnit(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index != null)
            {
                var indexToDivide = index.AddDataUnitIndex(dataUnitId);
                if (indexToDivide != null)
                {
                    DivideIndexByTwo(indexToDivide);
                }
                var filepath = index.FindIndexFilepathByUnitId(dataUnitId);
                DataUnitIndexDataSource.SaveIndexToFile(index);
                return filepath;
            }

            throw new Exception($"No Indexer for collection with id {collectionId} found!");
        }

        public void RemoveDataUnit(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index != null)
            {
                var indexToUnite = index.RemoveDataUnitIndex(dataUnitId);
                if (indexToUnite != null)
                {
                    UniteTwoIndex(indexToUnite);
                }
                DataUnitIndexDataSource.SaveIndexToFile(index);
            }
            else
            {
                throw new Exception($"No Indexer for collection with id {collectionId} found!");
            }
        }

        public List<string> GetAllIndexesDataFilePaths(string collectionId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index != null)
            {
                return index.GetAllIndexesFilePaths();
            }
            throw new Exception($"No indexer find for collection with id {collectionId}!");
        }

        public string GetDataUnitIndexFilepath(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index != null)
            {
                return index.FindIndexFilepathByUnitId(dataUnitId);
            }
            throw new Exception($"No index for collection with id {collectionId} found!");
        }

        protected IdIndex FindIndexByCollectionId(string collectionId)
        {
            return AllIndexes.FirstOrDefault(indexer => indexer.CollectionId == collectionId);
        }
        protected void DivideIndexByTwo(IdIndex indexToDivide)
        {
            var midId = SubIds(indexToDivide.GetRealMaxId(), indexToDivide.GetRealMinId());
            var dataFilepath = indexToDivide.GetFilepath();
            indexToDivide.Divide(midId);
            var leftFilepath = indexToDivide.Left.GetFilepath();
            var rightFilepath = indexToDivide.Right.GetFilepath();
            DataUnitDataSource.DivideIndexDataByTwo(dataFilepath, leftFilepath, rightFilepath, midId);
        }

        protected void UniteTwoIndex(IdIndex indexToUnite)
        {
            var dataFilepath = indexToUnite.GetFilepath();
            var leftFilepath = indexToUnite.Left.GetFilepath();
            var rightFilepath = indexToUnite.Right.GetFilepath();
            DataUnitDataSource.UniteDataIndex(dataFilepath, leftFilepath, rightFilepath);
            indexToUnite.Unite();
        }

        protected string SubIds(string firstId, string secId)
        {
            var firstIdBytes = SerializeUtils.StringToBytes(firstId);
            var secIdBytes = SerializeUtils.StringToBytes(secId);
            var bytes = new List<byte>();
            for (var i = 1; i < firstIdBytes.Count; i++)
            {
                var fByte = Convert.ToInt32(firstIdBytes[i]);
                var sByte = Convert.ToInt32(secIdBytes[i]);
                bytes.Add(SerializeUtils.IntToByte((fByte + sByte) / 2));
            }

            return new UTF8Encoding().GetString(bytes.ToArray());
        }
    }
}