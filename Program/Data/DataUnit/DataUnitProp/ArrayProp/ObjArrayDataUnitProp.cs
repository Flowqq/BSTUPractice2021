using System.Collections.Generic;

namespace Program
{
    public class ObjArrayDataUnitProp : DataUnitProp
    {
        public ObjArrayDataUnitProp(string name, List<DataUnit> value) : base(name, value, DataType.ObjArray)
        {
        }
    }
}