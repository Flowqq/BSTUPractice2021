using System.Collections.Generic;
using Program.DataPage;

namespace Program.userInterface
{
    public interface IUserInterface
    {
        List<CollectionDefinition> GetCollectionDefinitions();
        Collection CreateCollection(string collectionName);
        DataUnitsPaginator GetCollectionData(string collectionId, int pageSize = 10);
        DataUnit AddDataUnit(string collectionId, DataUnit dataUnit);
        DataUnit UpdateDataUnit(string collectionId, string dataUnitId, SortedSet<DataUnitProp> updatedProps);
        bool DeleteDataUnit(string collectionId, string dataUnitId);
        DataUnitsPaginator SearchDataUnits(string collectionId, SortedSet<DataUnitProp> searchFields, int pageSize = 10);
    }
}