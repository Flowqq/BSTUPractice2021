using System.Collections.Generic;

namespace Program
{
    public class DataUnitReference : DataUnit
    {
        public DataUnitReference(string id) : base(id)
        {
            fields.Add(new KeyValuePair<string, object>("collectionId", ""));
            fields.Add(new KeyValuePair<string, object>("unitId", ""));
        }
    }
}