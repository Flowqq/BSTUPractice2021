using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class StringDataUnitProp : DataUnitProp
    {
        public StringDataUnitProp(string name, string value) : base(name, value, DataType.String)
        {
        }

        public StringDataUnitProp(FileStream fileStream) : base(fileStream, DataType.String)
        {
        }

        protected override List<byte> SerializeValue()
        {
            var val = (string) Value;
            return SerializeUtils.StringToBytes(val);
        }

        protected override object DeserializeValue(FileStream fileStream)
        {
            return SerializeUtils.ReadNextString(fileStream);
        }
    }
}