using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class IntDataUnitProp : DataUnitProp
    {
        public IntDataUnitProp(string name, int value) : base(name, value, DataType.Integer)
        {
        }
        
        public IntDataUnitProp(FileStream fileStream) : base(fileStream, DataType.Integer)
        {
        }

        protected  override List<byte> SerializeValue()
        {
            var val = (int) Value;
            return new List<byte>() {SerializeUtils.IntToByte(val)};
        }

        protected  override object DeserializeValue(FileStream fileStream)
        {
            return SerializeUtils.ReadNextInt(fileStream);
        }
    }
}