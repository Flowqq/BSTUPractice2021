using System.Collections.Generic;

namespace Program
{
    public class RefDataUnitProp : DataUnitProp
    {
        public RefDataUnitProp(string name, DataUnitReference value) : base(name, value, DataType.Reference)
        {
        }
        protected override List<byte> GetValueBytes()
        {
            var val = (DataUnitReference) Value;
            return val.GetBytes();
        }
    }
}