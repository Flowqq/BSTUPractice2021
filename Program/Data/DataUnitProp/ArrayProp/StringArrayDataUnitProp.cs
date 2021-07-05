using System;
using System.Collections.Generic;

namespace Program
{
    public class StringArrayDataUnitProp : DataUnitProp
    {
        public StringArrayDataUnitProp(string name, List<string> value) : base(name, value, DataType.StringArray)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            List<byte> bytes = new List<byte>();
            var castedValue = (List<string>) Value;
            foreach (var strProp in castedValue)
            {
                bytes.AddRange(DataTypeUtils.StringToBytes(strProp));
            }

            bytes.Insert(0, Convert.ToByte(castedValue.Count));
            return bytes;
        }
    }
}