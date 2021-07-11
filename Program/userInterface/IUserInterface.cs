using System.Collections.Generic;
using Program.DataPage;

namespace Program.userInterface
{
    public interface IUserInterface
    {
        List<CollectionDefinition> GetCollectionDefinitions();

        CollectionDefinition CreateCollection(string collectionName);
        DataUnitsPaginator GetCollectionData(string collectionId, int pageSize = 10);
        DataUnit AddDataUnit(string collectionId, List<DataUnitProp> props);
        DataUnit UpdateDataUnit(string collectionId, DataUnit dataUnit);
        void DeleteDataUnit(string collectionId, string dataUnitId);
        DataUnitsPaginator SearchDataUnits(string collectionId, List<DataUnitProp> searchFields, int pageSize = 10);
    }
}