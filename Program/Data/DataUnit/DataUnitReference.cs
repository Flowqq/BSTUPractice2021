namespace Program
{
    public class DataUnitReference : DataUnit
    {
        public DataUnitReference(string id, string collectionId, string unitId) : base(id)
        {
            Props.Add(new StringDataUnitProp("collectionId", collectionId));
            Props.Add(new StringDataUnitProp("unitId", unitId));
        }
    }
}