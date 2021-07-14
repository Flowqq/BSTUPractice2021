using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class IntArrayDataUnitProp : DataUnitProp
    {
        public IntArrayDataUnitProp(string name, List<int> value) : base(name, value, DataType.IntArray)
        {
        }

        public IntArrayDataUnitProp(FileStream fileStream) : base(fileStream, DataType.IntArray)
        {
        }
        protected override List<byte> SerializeValue()
        {
            var val = (ICollection) Value;
            var serializeFunc = new Func<object, List<byte>>(val => new List<byte>(new []{SerializeUtils.IntToByte((int)val)}));
            return SerializeUtils.ArrayToBytes(val, serializeFunc);
        }

        protected  override object DeserializeValue(FileStream fileStream)
        {
            var deserializeFunc = new Func<FileStream, object>(fs => SerializeUtils.ReadNextInt(fs));
            var elements = SerializeUtils.ReadNextArray(fileStream, deserializeFunc);
            var castedElements = new List<int>();
            foreach (var element in elements)
            {
                castedElements.Add(Convert.ToInt32(element));
            }
            return castedElements;
        }
    }
}