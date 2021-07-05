using System;
using System.Collections.Generic;

namespace Program
{
    public class DoubleDataUnitProp : DataUnitProp
    {
        public DoubleDataUnitProp(string name, double value) : base(name, value, DataType.Double)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            return new() {Convert.ToByte(Value)};
        }
    }
}