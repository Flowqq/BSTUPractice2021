using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IIndexFileInterface
    {
        void SaveIndexToFile(IdIndex index);
        public List<IdIndex> LoadIndexes(List<CollectionDefinition> colDefs);
        void CreateIndex(CollectionDefinition collectionDefinition);
    }
}