using System.Collections.Generic;

namespace Program
{
    public class StringDataUnitProp : DataUnitProp
    {
        public StringDataUnitProp(string name, string value) : base(name, value, DataType.String)
        {
        }

        protected override List<byte> GetValueBytes()
        {
            var castedValue = (string) Value;
            return DataTypeUtils.StringToBytes(castedValue);
        }
    }
}