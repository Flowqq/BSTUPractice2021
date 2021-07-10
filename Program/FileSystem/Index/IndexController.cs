using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Program.userInterface;

namespace Program
{
    public class IndexController
    {
        public IDataUnitFileInterface DataUnitFileInterface { get; }
        public IIndexFileInterface IndexFileInterface { get; }
        public List<IdIndex> AllIndexes { get; }

        public IndexController(List<CollectionDefinition> colDefs, IDataUnitFileInterface dataUnitFileInterface, IIndexFileInterface indexFileInterface)
        {
            DataUnitFileInterface = dataUnitFileInterface;
            IndexFileInterface = indexFileInterface;
            AllIndexes = IndexFileInterface.LoadIndexes(colDefs);
        }
        
        public void CreateIndex(CollectionDefinition collectionDefinition)
        {
            var newIndex = new IdIndex(collectionDefinition);
            IndexFileInterface.CreateIndex(collectionDefinition);
            IndexFileInterface.SaveIndexToFile(newIndex);
            AllIndexes.Add(newIndex);
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
                var filepath = index.FindDataUnitFilePathById(dataUnitId);
                IndexFileInterface.SaveIndexToFile(index);
                return filepath;
            }

            throw new Exception($"No Indexer for collection with id {collectionId} found!");
        }

        public string RemoveDataUnit(string collectionId, string dataUnitId)
        {
            var index = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (index != null)
            {
                var filepath = FileSystemConfig.GetCollectionDataFilepathByIndex(index.RemoveDataUnitIndex(dataUnitId));
                IndexFileInterface.SaveIndexToFile(index);
                return filepath;
            }

            throw new Exception($"No Indexer for collection with id {collectionId} found!");
        }

        public List<string> GetAllIndexesDataFilePaths(string collectionId)
        {
            var index = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (index != null)
            {
                return index.GetFilePaths();
            }

            throw new Exception($"No indexer find for collection with id {collectionId}!");
        }

        public string GetDataUnitIndexFilepath(string collectionId, string dataUnitId)
        {
            var indexter = AllIndexes.FirstOrDefault(indexer => indexer.ColDef.Id == collectionId);
            if (indexter != null)
            {
                return indexter.FindDataUnitFilePathById(dataUnitId);
            }

            return null;
        }

        protected void DivideByTwo(IdIndex indexToDivide)
        {
            var midId = SubIds(indexToDivide.GetMaxRealId(), indexToDivide.GetMinRealId());
            var dataFilepath = FileSystemConfig.GetCollectionDataFilepathByIndex(indexToDivide);
            // запихнуть в IdIndex
            var loverIds = indexToDivide.IdList.Where(id => String.Compare(id, midId, StringComparison.Ordinal) < 0);
            var upperIds = indexToDivide.IdList.Where(id => String.Compare(id, midId, StringComparison.Ordinal) >= 0);

            var leftPostfix = indexToDivide.FilePostfix + "L";
            var rightPostfix = indexToDivide.FilePostfix + "R";
            var left = new IdIndex(new List<string>(loverIds), indexToDivide.MinId, midId, indexToDivide.MaxElements,
                leftPostfix, indexToDivide.ColDef);
            var right = new IdIndex(new List<string>(upperIds), midId, indexToDivide.MaxId, indexToDivide.MaxElements,
                rightPostfix, indexToDivide.ColDef);
            indexToDivide.Divide(left, right);
            try
            {
                var leftFilepath = FileSystemConfig.GetCollectionDataFilepathByIndex(left);
                var rightFilepath = FileSystemConfig.GetCollectionDataFilepathByIndex(right);
                DataUnitFileInterface.DivideIndexDataByTwo(dataFilepath, leftFilepath, rightFilepath, midId);
                indexToDivide.IdList.Clear();
                IndexFileInterface.SaveIndexToFile(indexToDivide);
            }
            catch (Exception e)
            {
                indexToDivide.Divide(null, null);
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