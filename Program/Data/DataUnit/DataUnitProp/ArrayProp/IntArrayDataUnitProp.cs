using System.Collections.Generic;

namespace Program
{
    public class IntArrayDataUnitProp : DataUnitProp
    {
        public IntArrayDataUnitProp(string name, List<int> value) : base(name, value, DataType.IntArray)
        {
        }
    }
}