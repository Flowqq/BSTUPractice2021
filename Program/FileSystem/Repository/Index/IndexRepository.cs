using System.Collections.Generic;
using System.Linq;
using Program.Exceptions;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class IndexRepository
    {
        protected IDataUnitIndexDataSource DataUnitIndexDataSource { get; }
        public List<IdIndex> AllIndexes { get; }

        public IndexRepository(List<CollectionDefinition> colDefs,
            IDataUnitIndexDataSource dataUnitIndexDataSource)
        {
            DataUnitIndexDataSource = dataUnitIndexDataSource;
            AllIndexes = DataUnitIndexDataSource.LoadIndexes(colDefs);
        }

        public void CreateIndex(string collectionId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index == null)
            {
                var newIndex = new IdIndex(collectionId);
                DataUnitIndexDataSource.CreateIndex(collectionId);
                DataUnitIndexDataSource.UpdateIndexFile(newIndex);
                AllIndexes.Add(newIndex);
                var firstDataFilePath = newIndex.GetAllIndexesFilePaths().FirstOrDefault();
                DirUtils.CreateFile(firstDataFilePath);
            }
        }

        public IdIndex RemoveIndex(string collectionId)
        {
            var indexToRemove = FindIndexByCollectionId(collectionId);
            AllIndexes.Remove(indexToRemove);
            return indexToRemove;
        }
        public void AddIndex(IdIndex index)
        {
            AllIndexes.Add(index);
        }

        public IdIndex AddDataUnit(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            var indexToDivide = index.AddDataUnitIndex(dataUnitId);
            if (indexToDivide != null)
            {
                indexToDivide.Divide();
            }

            DataUnitIndexDataSource.UpdateIndexFile(index);
            return indexToDivide;
        }

        public IdIndex RemoveDataUnit(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            var indexToUnite = index.RemoveDataUnitIndex(dataUnitId);
            if (indexToUnite != null)
            {
                indexToUnite.Unite();
            }

            DataUnitIndexDataSource.UpdateIndexFile(index);
            return indexToUnite;
        }

        public List<string> GetAllIndexesDataFilePaths(string collectionId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            return index.GetAllIndexesFilePaths();
        }

        public string GetDataUnitIndexFilepath(string collectionId, string dataUnitId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            return index.FindIndexFilepathByUnitId(dataUnitId);
        }

        protected IdIndex FindIndexByCollectionId(string collectionId)
        {
            return AllIndexes.FirstOrDefault(indexer => indexer.CollectionId == collectionId);
        }

        protected  IdIndex FindIndexByCollectionIdOrThrow(string collectionId)
        {
            var index = FindIndexByCollectionId(collectionId);
            if (index != null)
            {
                return index;
            }

            throw IndexNotFoundException.GenerateException(collectionId);
        }

        public void MakeBackupOfIndex(string collectionId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            var backupFilepath = PathUtils.GetIndexBackupFilepath(collectionId);
            DataUnitIndexDataSource.SaveIndexToFile(backupFilepath, index);
        }

        public void LoadBackupOfIndex(string collectionId)
        {
            var index = FindIndexByCollectionIdOrThrow(collectionId);
            var backupFilepath = PathUtils.GetIndexBackupFilepath(collectionId);
            var loadedIndex = DataUnitIndexDataSource.LoadIndexFromFile(backupFilepath, collectionId);
            AllIndexes.Remove(index);
            AllIndexes.Add(loadedIndex);
            DataUnitIndexDataSource.UpdateIndexFile(loadedIndex);
        }
    }
}