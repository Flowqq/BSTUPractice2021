using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IDataUnitIndexDataSource
    {
        void UpdateIndexFile(IdIndex index);
        public List<IdIndex> LoadIndexes(List<CollectionDefinition> colDefs);
        void CreateIndex(string collectionId);
        public void SaveIndexToFile(string filepath, IdIndex index);
        IdIndex LoadIndexFromFile(string filepath, string collectionId);

    }
}