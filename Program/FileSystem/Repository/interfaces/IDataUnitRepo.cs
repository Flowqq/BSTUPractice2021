using System.Collections.Generic;
using Program.DataPage;

namespace Program.Controller.interfaces
{
    public interface IDataUnitRepo
    {
        public DataUnitsPaginator LoadCollectionData(string collectionId, int pageSize = 10);
        public DataUnit GetDataUnitById(string collectionId, string dataUnitId);
        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props);
        public void SaveDataUnit(string collectionId, DataUnit dataUnit);
        public void DeleteDataUnit(string collectionId, string dataUnitId);
    }
}