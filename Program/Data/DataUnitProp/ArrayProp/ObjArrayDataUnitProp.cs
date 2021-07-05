using System;
using System.Collections.Generic;

namespace Program
{
    public class ObjArrayDataUnitProp : DataUnitProp
    {
        public ObjArrayDataUnitProp(string name, List<DataUnit> value) : base(name, value, DataType.ObjArray)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            List<byte> bytes = new List<byte>();
            var castedValue = (List<DataUnit>) Value;
            foreach (var dataUnit in castedValue)
            {
                bytes.AddRange(dataUnit.GetBytes());
            }
            bytes.Insert(0, Convert.ToByte(castedValue.Count));
            return bytes;
        }
    }
}