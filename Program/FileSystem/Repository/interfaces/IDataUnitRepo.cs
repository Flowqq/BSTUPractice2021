using System.Collections.Generic;

namespace Program.Controller.interfaces
{
    public interface IDataUnitRepo
    {
        public List<DataUnit> LoadCollectionData(string collectionId);
        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props);
        public List<DataUnit> GetDataUnitsByPropsAllCollections(List<DataUnitProp> props);
        public void SaveDataUnit(string collectionId, DataUnit dataUnit);
        public void DeleteDataUnit(string collectionId, string dataUnitId);
    }
}