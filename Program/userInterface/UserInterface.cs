using System.Collections.Generic;
using Program.DataPage;

namespace Program.userInterface
{
    public class UserInterface : IUserInterface
    {
        protected MainRepo MainRepo = new MainRepo();
        
        public List<CollectionDefinition> GetCollectionDefinitions()
        {
            return MainRepo.CollectionDefinitionRepository.LoadCollectionDefinitions();
        }

        public CollectionDefinition CreateCollection(string collectionName)
        {
            var id = SerializeUtils.GenerateId();
            var colDef = new CollectionDefinition(id, collectionName, 0);
            MainRepo.CollectionDefinitionRepository.CreateCollection(colDef);
            return colDef;
        }

        public DataUnitsPaginator GetCollectionData(string collectionId, int pageSize = 10)
        {
            return MainRepo.DataUnitRepository.LoadCollectionData(collectionId, pageSize);
        }

        public DataUnit AddDataUnit(string collectionId, List<DataUnitProp> props)
        {
            var id = SerializeUtils.GenerateId();
            var dataUnit = new DataUnit(id, props);
            MainRepo.DataUnitRepository.SaveDataUnit(collectionId, dataUnit);
            return dataUnit;
        }

        public DataUnit UpdateDataUnit(string collectionId, DataUnit dataUnit)
        {
            MainRepo.DataUnitRepository.SaveDataUnit(collectionId, dataUnit);
            return dataUnit;
        }

        public void DeleteDataUnit(string collectionId, string dataUnitId)
        {
            MainRepo.DataUnitRepository.DeleteDataUnit(collectionId, dataUnitId);
        }

        public DataUnitsPaginator SearchDataUnits(string collectionId, List<DataUnitProp> searchFields, int pageSize = 10)
        {
            var dataUnits = MainRepo.DataUnitRepository.GetDataUnitsByProps(collectionId, searchFields);
            return new DataUnitsPaginator(pageSize, dataUnits);
        }
    }
}