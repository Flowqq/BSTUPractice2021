using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public class Collection
    {
        public string Id { get; }
        public string Name { get; }
        public Dictionary<DataType, HashSet<DataUnit>> DataUnits { get; }

        public Collection(string id, string name)
        {
            Id = id;
            Name = name;
            DataUnits = new Dictionary<DataType, HashSet<DataUnit>>();
        }

        public override bool Equals(object obj)
        {
            return obj is Collection collection &&
                   Id == collection.Id &&
                   Name == collection.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
}