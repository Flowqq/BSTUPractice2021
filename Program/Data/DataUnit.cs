﻿using System;
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
            return obj is DataUnit unit &&
                   Id == unit.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}