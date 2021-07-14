using System;
using System.Collections.Generic;
using Program.DataPage;

namespace Program.userInterface
{
    public interface IUserInterface
    {
        List<CollectionDefinition> GetCollectionDefinitions();

        CollectionDefinition CreateCollection(string collectionName);
        CollectionDefinition RenameCollection(string collectionId, string newName);
        void DeleteCollection(string collectionId);
        DataUnitsPaginator GetCollectionData(string collectionId, Comparison<DataUnit> sortFunc = null, int pageSize = 10);
        DataUnitsPaginator SearchDataUnits(string collectionId, List<DataUnitProp> searchFields, Comparison<DataUnit> sortFunc = null, int pageSize = 10);
        DataUnitsPaginator SearchDataUnitsAllCollections(List<DataUnitProp> searchFields, Comparison<DataUnit> sortFunc = null, int pageSize = 10);

        DataUnit AddDataUnit(string collectionId);
        DataUnit UpdateDataUnit(string collectionId, DataUnit dataUnit);
        void DeleteDataUnit(string collectionId, string dataUnitId);
    }
}