using System;
using System.Collections.Generic;

namespace Program
{
    public class DoubleArrayDataUnitProp : DataUnitProp
    {
        public DoubleArrayDataUnitProp(string name, List<double> value) : base(name, value, DataType.DoubleArray)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            List<byte> bytes = new List<byte>();
            var castedValue = (List<double>) Value;
            foreach (var dataUnitVal in castedValue)
            {
                bytes.Add(Convert.ToByte(dataUnitVal));
            }
            bytes.Insert(0, Convert.ToByte(castedValue.Count));
            return bytes;
        }
    }
}