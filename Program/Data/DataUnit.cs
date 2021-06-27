using System;
using System.Collections.Generic;

namespace Program
{
    public class DataUnit
    {
        public string Id { get; }
        public Dictionary<string, object> Props { get; set; }

        public DataUnit(string id)
        {
            Id = id;
            Props = new Dictionary<string, object>();
        }

        public override bool Equals(object obj)
        {
            return obj is DataUnit unit &&
                   Id == unit.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}