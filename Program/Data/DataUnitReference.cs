namespace Program
{
    public class DataUnitReference : DataUnit
    {
        public DataUnitReference(string id) : base(id)
        {
            Props.Add("collectionId", "");
            Props.Add("unitId", "");
        }
    }
}