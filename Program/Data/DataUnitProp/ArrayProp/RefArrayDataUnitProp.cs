using System;
using System.Collections.Generic;

namespace Program
{
    public class RefArrayDataUnitProp : DataUnitProp
    {
        public RefArrayDataUnitProp(string name, List<DataUnitReference> value) : base(name, value, DataType.RefArray)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            List<byte> bytes = new List<byte>();
            var castedValue = (List<DataUnitReference>) Value;
            foreach (var dataUnit in castedValue)
            {
                bytes.AddRange(dataUnit.GetBytes());
            }
            bytes.Insert(0, Convert.ToByte(castedValue.Count));
            return bytes;
        }
    }
}