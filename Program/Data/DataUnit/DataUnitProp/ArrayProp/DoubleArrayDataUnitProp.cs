using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class DoubleArrayDataUnitProp : DataUnitProp
    {
        public DoubleArrayDataUnitProp(string name, List<double> value) : base(name, value, DataType.DoubleArray)
        {
        }

        public DoubleArrayDataUnitProp(FileStream fileStream) : base(fileStream, DataType.DoubleArray)
        {
        }

        protected override List<byte> SerializeValue()
        {
            var val = (ICollection) Value;
            var serializeFunc = new Func<object, List<byte>>(val => SerializeUtils.DoubleToBytes((double)val));
            return SerializeUtils.ArrayToBytes(val, serializeFunc);
        }

        protected override object DeserializeValue(FileStream fileStream)
        {
            var deserializeFunc = new Func<FileStream, object>(fs => SerializeUtils.ReadNextDouble(fs));
            var elements = SerializeUtils.ReadNextArray(fileStream, deserializeFunc);
            var castedElements = new List<double>();
            foreach (var element in elements)
            {
                castedElements.Add(Convert.ToDouble(element));
            }
            return castedElements;
        }
    }
}