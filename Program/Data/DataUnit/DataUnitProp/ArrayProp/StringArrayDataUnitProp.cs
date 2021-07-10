using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class StringArrayDataUnitProp : DataUnitProp
    {
        public StringArrayDataUnitProp(string name, List<string> value) : base(name, value, DataType.StringArray)
        {
        }

        public StringArrayDataUnitProp(FileStream fileStream) : base(fileStream, DataType.StringArray)
        {
        }
        protected  override List<byte> SerializeValue()
        {
            var val = (ICollection) Value;
            var serializeFunc = new Func<object, List<byte>>(val => SerializeUtils.StringToBytes((string)val));
            return SerializeUtils.ArrayToBytes(val, serializeFunc);
        }

        protected  override object DeserializeValue(FileStream fileStream)
        {
            var deserializeFunc = new Func<FileStream, object>(SerializeUtils.ReadNextString);
            return SerializeUtils.ReadNextArray(fileStream, deserializeFunc);
        }
    }
}