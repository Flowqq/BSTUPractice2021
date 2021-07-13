using System;
using System.Collections.Generic;
using System.IO;

namespace Program
{
    public abstract class DataUnitProp : IComparable
    {
        public string Name { get; }
        public object Value { get; }
        public DataType Type { get; }

        public DataUnitProp(string name, object value, DataType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public DataUnitProp(FileStream fileStream, DataType type)
        {
            Name = SerializeUtils.ReadNextString(fileStream);
            Type = type;
            Value = DeserializeValue(fileStream);
        }

        public List<byte> Serialize()
        {
            var bytes = new List<byte>();
            bytes.Add(SerializeUtils.IntToByte((int)Type));
            bytes.AddRange(SerializeUtils.StringToBytes(Name));
            bytes.AddRange(SerializeValue());
            return bytes;
        }
        protected abstract List<byte> SerializeValue();
        protected abstract object DeserializeValue(FileStream fileStream);
        protected bool Equals(DataUnitProp other)
        {
            return Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataUnitProp) obj);
        }
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public int CompareTo(object? obj)
        {
            if (obj != null)
            {
                var val = (DataUnitProp) obj;
                return String.Compare(Name, val.Name, StringComparison.Ordinal);
            }
            else
            {
                return -1;
            }
        }
    }
}