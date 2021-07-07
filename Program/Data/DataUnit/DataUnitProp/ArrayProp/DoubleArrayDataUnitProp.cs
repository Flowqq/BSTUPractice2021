using System.Collections.Generic;

namespace Program
{
    public class DoubleArrayDataUnitProp : DataUnitProp
    {
        public DoubleArrayDataUnitProp(string name, List<double> value) : base(name, value, DataType.DoubleArray)
        {
        }
    }
}