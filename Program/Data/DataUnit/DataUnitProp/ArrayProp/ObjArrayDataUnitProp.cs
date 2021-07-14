using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class ObjArrayDataUnitProp : DataUnitProp
    {
        public ObjArrayDataUnitProp(string name, List<DataUnit> value) : base(name, value, DataType.ObjArray)
        {
        }

        public ObjArrayDataUnitProp(FileStream fileStream) : base(fileStream, DataType.ObjArray)
        {
        }
        protected  override List<byte> SerializeValue()
        {
            var val = (ICollection) Value;
            var serializeFunc = new Func<object, List<byte>>(value =>
            {
                var val = (DataUnit) value;
                return val.Serialize();
            });
            return SerializeUtils.ArrayToBytes(val, serializeFunc);
        }

        protected  override object DeserializeValue(FileStream fileStream)
        {
            var deserializeFunc = new Func<FileStream, object>(DataUnit.Deserialize);
            return SerializeUtils.ReadNextArray(fileStream, deserializeFunc);
        }
    }
}