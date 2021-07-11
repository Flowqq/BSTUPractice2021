using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Program
{
    public class DataUnit : IComparable
    {
        public string Id { get; set; }
        public List<DataUnitProp> Props { get; }

        public DataUnit(string id)
        {
            Id = id;
            Props = new List<DataUnitProp>();
        }
        public DataUnit(string id, List<DataUnitProp> props)
        {
            Id = id;
            Props = props;
        }
        public void Update(SortedSet<DataUnitProp> updatedProps)
        {
            foreach (var prop in updatedProps)
            {
                if (!Props.Contains(prop))
                {
                    AddProperty(prop);
                }
                else
                {
                    UpdateProperty(prop);
                }
            }
        }

        public DataUnitProp GetProperty(string name)
        {
            return Props.FirstOrDefault(prop => prop.Name == name);
        }

        public void AddProperty(DataUnitProp dataUnitProp)
        {
            Props.Add(dataUnitProp);
        }
        public void AddProperties(HashSet<DataUnitProp> dataUnitProps)
        {
            foreach (var dataUnit in dataUnitProps)
            {
                Props.Add(dataUnit);
            }
        }

        public void UpdateProperty(DataUnitProp dataUnitProp)
        {
            SetProperty(dataUnitProp.Name, dataUnitProp);
        }
        protected void SetProperty(string name, DataUnitProp dataUnitProp)
        {
            var propToUpdate = GetProperty(name);
            if (propToUpdate != null)
            {
                var removed = Props.Remove(propToUpdate);
                Props.Add(dataUnitProp);
            }
        }

        public List<byte> Serialize()
        {
            var bytes = new List<byte>();
            bytes.AddRange(SerializeUtils.StringToBytes(Id));
            var propsCount = Props.Count;
            bytes.Add(SerializeUtils.IntToByte(propsCount));
            foreach (var prop in Props)
            {
                bytes.AddRange(prop.Serialize());
            }
            return bytes;
        }

        public static DataUnit Deserialize(FileStream fileStream)
        {
            var id = SerializeUtils.ReadNextString(fileStream);
            var propsCount = SerializeUtils.ReadNextInt(fileStream);
            var props = new List<DataUnitProp>();
            for (var i = 0; i < propsCount; i++)
            {
                var prop = DataUnitPropFactory.DeserializeDataUnit(fileStream);
                props.Add(prop);
            }
            return new DataUnit(id, props);
        }
        protected bool Equals(DataUnit other)
        {
            return Id == other.Id;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataUnit) obj);
        }
        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public int CompareTo(object? obj)
        {
            if (obj != null)
            {
                var val = (DataUnit) obj;
                return Id.CompareTo(val.Id);
            }
            else
            {
                return -1;
            }
        }
    }
}