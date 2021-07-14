using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class ObjectDataUnitProp : DataUnitProp
    {
        public ObjectDataUnitProp(string name, DataUnit value) : base(name, value, DataType.Object)
        {
        }

        public ObjectDataUnitProp(FileStream fileStream) : base(fileStream, DataType.Object)
        {
        }

        protected override List<byte> SerializeValue()
        {
            var val = (DataUnit) Value;
            return val.Serialize();
        }

        protected  override object DeserializeValue(FileStream fileStream)
        {
            return DataUnit.Deserialize(fileStream);
        }
    }
}