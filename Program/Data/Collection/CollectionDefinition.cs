using System.Collections.Generic;
using System.IO;

namespace Program
{
    public class CollectionDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public int DataUnitsCount { get; }

        public CollectionDefinition(string id, string name, int dataUnitsCount)
        {
            Id = id;
            Name = name;
            DataUnitsCount = dataUnitsCount;
        }

        public Collection ToEmptyCollection()
        {
            return new Collection(Id, Name);
        }
        public List<byte> Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(SerializeUtils.StringToBytes(Id));
            bytes.AddRange(SerializeUtils.StringToBytes(Name));
            bytes.Add(SerializeUtils.IntToByte(DataUnitsCount));
            return bytes;
        }

        public static CollectionDefinition Deserialize(FileStream fileStream)
        {
            var id = SerializeUtils.ReadNextString(fileStream);
            var name = SerializeUtils.ReadNextString(fileStream);
            var dataUnitsCount = SerializeUtils.ReadNextInt(fileStream);
            return new CollectionDefinition(id, name, dataUnitsCount);
        }
    }
}