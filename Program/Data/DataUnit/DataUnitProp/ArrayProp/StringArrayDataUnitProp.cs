using System.Collections.Generic;

namespace Program
{
    public class StringArrayDataUnitProp : DataUnitProp
    {
        public StringArrayDataUnitProp(string name, List<string> value) : base(name, value, DataType.StringArray)
        {
        }
    }
}