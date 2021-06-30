using System.Collections.Generic;
using Program.DataPage;

namespace Program.userInterface
{
    public interface IUserInterface
    {
        Collection CreateCollection(string collectionName);
        DataUnitsPaginator GetCollectionData(string collectionId);
        DataUnit AddDataUnit(string collectionId, DataUnit dataUnit);
        DataUnit UpdateDataUnit(string collectionId, string dataUnitId, SortedSet<DataUnitProp> updatedProps);
        bool DeleteDataUnit(string collectionId, string dataUnitId);
        DataUnitsPaginator SearchDataUnits(string collectionId, SortedSet<DataUnitProp> searchFields);
    }
}