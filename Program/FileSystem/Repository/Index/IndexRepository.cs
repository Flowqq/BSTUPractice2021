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
        protected List<IdIndex> AllIndexes { get; }

        public IndexRepository(List<CollectionDefinition> colDefs, IDataUnitDataSource dataUnitDataSource,
            IDataUnitIndexDataSource dataUnitIndexDataSource)
        {
            DataUnitDataSource = dataUnitDataSource;
            DataUnitIndexDataSource = dataUnitIndexDataSource;
            AllIndexes = DataUnitIndexDataSource.LoadIndexes(colDefs);
        }

        public void CreateIndex(CollectionDefinition collectionDefinition)
        {
            if (AllIndexes.Find(index => index.ColDef.Id == collectionDefinition.Id) == null)
            {
                var newIndex = new IdIndex(collectionDefinition);
                DataUnitIndexDataSource.CreateIndex(collectionDefinition);
                DataUnitIndexDataSource.SaveIndexToFile(newIndex);
                AllIndexes.Add(newIndex);
            }
        }

        public void RemoveIndex(string collectionId)
        {
            AllIndexes.RemoveAll(index => index.ColDef.Id == collectionId);
        }

        public string AddDataUnit(string collectionId, string dataUnitId)
        {
            var index = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (index != null)
            {
                var indexToDivide = index.AddDataUnitIndex(dataUnitId);
                if (indexToDivide != null)
                {
                    DivideByTwo(index);
                }
                var filepath = index.FindIndexFilepathByUnitId(dataUnitId);
                DataUnitIndexDataSource.SaveIndexToFile(index);
                return filepath;
            }

            throw new Exception($"No Indexer for collection with id {collectionId} found!");
        }

        public string RemoveDataUnit(string collectionId, string dataUnitId)
        {
            var index = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (index != null)
            {
                var filepath = index.RemoveDataUnitIndex(dataUnitId).GetFilepath();
                DataUnitIndexDataSource.SaveIndexToFile(index);
                return filepath;
            }

            throw new Exception($"No Indexer for collection with id {collectionId} found!");
        }

        public List<string> GetAllIndexesDataFilePaths(string collectionId)
        {
            var index = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (index != null)
            {
                return index.GetAllIndexesFilePaths();
            }

            throw new Exception($"No indexer find for collection with id {collectionId}!");
        }

        public string GetDataUnitIndexFilepath(string collectionId, string dataUnitId)
        {
            var indexter = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (indexter != null)
            {
                return indexter.FindIndexFilepathByUnitId(dataUnitId);
            }
            return null;
        }

        protected void DivideByTwo(IdIndex indexToDivide)
        {
            var indexCopy = new IdIndex(new HashSet<string>(indexToDivide.IdList), indexToDivide.MinId, indexToDivide.MaxId, indexToDivide.MaxElements, indexToDivide.FileName, indexToDivide.ColDef);
            var midId = SubIds(indexToDivide.GetMaxRealId(), indexToDivide.GetMinRealId());
            var dataFilepath = indexToDivide.GetFilepath();
            indexToDivide.Divide(midId);
            var leftFilepath = indexToDivide.Left.GetFilepath();
            var rightFilepath = indexToDivide.Right.GetFilepath();
            try
            {
                DataUnitDataSource.DivideIndexDataByTwo(dataFilepath, leftFilepath, rightFilepath, midId);
                DataUnitIndexDataSource.SaveIndexToFile(indexToDivide);
            }
            catch (Exception e)
            {
                indexToDivide = indexCopy;
                throw;
            }
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