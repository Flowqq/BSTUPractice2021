using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IUserInterface
    {
        Collection AddCollection(string collectionName);
        Dictionary<DataType, SortedSet<DataUnit>> GetCollectionData(string collectionId);
        DataUnit AddDataUnit(string collectionId, DataUnit dataUnit);
        DataUnit UpdateDataUnit(string collectionId, string dataUnitId, SortedSet<DataUnitProp> updatedProps);
        bool DeleteDataUnit(string collectionId, string dataUnitId);
        SortedSet<DataUnit> SearchDataUnits(string collectionId, SortedSet<DataUnit> searchFields);
    }
}