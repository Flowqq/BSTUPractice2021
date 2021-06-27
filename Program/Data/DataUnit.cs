using System;
using System.Collections.Generic;

namespace Program
{
    public class DataUnit
    {
        public string id;
        public List<KeyValuePair<string, object>> fields;

        public DataUnit(string id)
        {
            this.id = id;
            this.fields = new List<KeyValuePair<string, object>>();
        }
        public override bool Equals(object obj)
        {
            return obj is DataUnit unit &&
                   id == unit.id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(id);
        }
    }
}