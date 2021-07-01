using System;
using System.Collections.Generic;

namespace Program
{
    public class Collection
    {
        public string Id { get; }
        public string Name { get; }
        public SortedSet<DataUnit> DataUnits { get; }

        public Collection(string id, string name)
        {
            Id = id;
            Name = name;
            DataUnits = new SortedSet<DataUnit>();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Collection) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}