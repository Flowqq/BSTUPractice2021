using System.Collections.Generic;

namespace Program.Controller.interfaces
{
    public interface IMainRepo
    {
        public List<DataUnit> LoadCollectionData(string collectionId);
        public List<DataUnit> GetDataUnitsByProps(string collectionId, List<DataUnitProp> props);
        public List<DataUnit> GetDataUnitsByPropsAllCollections(List<DataUnitProp> props);
        public void SaveDataUnit(string collectionId, DataUnit dataUnit);
        public void DeleteDataUnit(string collectionId, string dataUnitId);

        public List<CollectionDefinition> LoadCollectionDefinitions();
        public void SaveCollection(CollectionDefinition collectionDefinition);
        public void CreateCollection(CollectionDefinition collectionDefinition);
        public void DeleteCollection(string collectionId);
    }
}