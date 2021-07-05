using System;
using System.Collections.Generic;

namespace Program
{
    public class IntDataUnitProp : DataUnitProp
    {
        public IntDataUnitProp(string name, int value) : base(name, value, DataType.Integer)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            return new() {Convert.ToByte(Value)};
        }
    }
}