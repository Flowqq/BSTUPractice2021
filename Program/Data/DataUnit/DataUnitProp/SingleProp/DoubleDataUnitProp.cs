using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class DoubleDataUnitProp : DataUnitProp
    {
        public DoubleDataUnitProp(string name, double value) : base(name, value, DataType.Double)
        {
        }

        public DoubleDataUnitProp(FileStream fileStream) : base(fileStream, DataType.Double)
        {
        }

        protected  override List<byte> SerializeValue()
        {
            var val = (double) Value;
            return SerializeUtils.DoubleToBytes(val);
        }
        protected  override object DeserializeValue(FileStream fileStream)
        {
            return SerializeUtils.ReadNextDouble(fileStream);
        }
    }
}