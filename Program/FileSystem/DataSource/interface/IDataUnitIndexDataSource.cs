using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IDataUnitIndexDataSource
    {
        void SaveIndexToFile(IdIndex index);
        public List<IdIndex> LoadIndexes(List<CollectionDefinition> colDefs);
        void CreateIndex(CollectionDefinition collectionDefinition);
    }
}