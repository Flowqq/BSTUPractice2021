using System;
using System.Collections.Generic;

namespace Program
{
    public class IntArrayDataUnitProp : DataUnitProp
    {
        public IntArrayDataUnitProp(string name, List<int> value) : base(name, value, DataType.IntArray)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            List<byte> bytes = new List<byte>();
            var castedValue = (List<int>) Value;
            foreach (var dataUnitVal in castedValue)
            {
                bytes.Add(Convert.ToByte(dataUnitVal));
            }
            bytes.Insert(0, Convert.ToByte(castedValue.Count));
            return bytes;
        }
    }
}