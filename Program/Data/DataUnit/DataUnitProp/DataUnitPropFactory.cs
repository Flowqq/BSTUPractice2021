using System;
using System.IO;

namespace Program
{
    public class DataUnitPropFactory
    {
        public static DataUnitProp DeserializeDataUnit(FileStream fileStream)
        {
            var type = (DataType) fileStream.ReadByte();
            return DeserializePropWithType(type, fileStream);
        }
        public static DataUnitProp DeserializePropWithType(DataType dataType, FileStream fileStream)
        {
            return dataType switch
            {
                DataType.Integer => new IntDataUnitProp(fileStream),
                DataType.String => new StringDataUnitProp(fileStream),
                DataType.Object => new ObjectDataUnitProp(fileStream),
                DataType.Double => new DoubleDataUnitProp(fileStream),
                DataType.IntArray => new IntArrayDataUnitProp(fileStream),
                DataType.DoubleArray => new DoubleArrayDataUnitProp(fileStream),
                DataType.StringArray => new StringArrayDataUnitProp(fileStream),
                DataType.ObjArray => new ObjArrayDataUnitProp(fileStream),
                _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType,
                    $"No prop class for this DataType ({dataType}) defined!")
            };
        }
    }
}