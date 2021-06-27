using System;
using System.Collections.Generic;

namespace Program
{
    public class Collection
    {
        public string id;
        public string name;
        public Dictionary<DataType, List<DataUnit>> dataUnits;

        public Collection(string id, string name)
        {
            this.id = id;
            this.name = name;
            this.dataUnits = new Dictionary<DataType, List<DataUnit>>();
        }

        public override bool Equals(object obj)
        {
            return obj is Collection collection &&
                   id == collection.id &&
                   name == collection.name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, name);
        }
    }
}