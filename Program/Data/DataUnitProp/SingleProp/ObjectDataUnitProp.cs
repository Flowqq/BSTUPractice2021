using System.Collections.Generic;

namespace Program
{
    public class ObjectDataUnitProp : DataUnitProp
    {
        public ObjectDataUnitProp(string name, DataUnit value) : base(name, value, DataType.Object)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            var val = (DataUnit) Value;
            return val.GetBytes();
        }
    }
}