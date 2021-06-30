﻿using System;
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