using System.Collections.Generic;

namespace Program
{
    public class ArrayDataUnitProp : DataUnitProp
    {
        public ArrayDataUnitProp(string name, List<DataUnit> value) : base(name, value, DataType.Array)
        {
        }
    }
}