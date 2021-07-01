using System;
using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public class DataUnit
    {
        public string Id { get; }
        public SortedSet<DataUnitProp> Props { get; set; }

        public DataUnit(string id)
        {
            Id = id;
            Props = new SortedSet<DataUnitProp>();
        }

        public DataUnitProp GetProperty(string name)
        {
            return Props.FirstOrDefault(prop => prop.Name == name);
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
    }
}