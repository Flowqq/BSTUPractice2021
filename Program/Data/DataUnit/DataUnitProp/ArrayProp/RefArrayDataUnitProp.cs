using System.Collections.Generic;

namespace Program
{
    public class RefArrayDataUnitProp : DataUnitProp
    {
        public RefArrayDataUnitProp(string name, List<DataUnitReference> value) : base(name, value, DataType.RefArray)
        {
        }
    }
}