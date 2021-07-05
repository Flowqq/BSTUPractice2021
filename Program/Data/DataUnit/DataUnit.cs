using System.Collections.Generic;
using System.Linq;

namespace Program
{
    public class DataUnit
    {
        public string Id { get; }
        public SortedSet<DataUnitProp> Props { get; }

        public DataUnit(string id)
        {
            Id = id;
            Props = new SortedSet<DataUnitProp>();
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

        public bool AddProperty(DataUnitProp dataUnitProp)
        {
            return Props.Add(dataUnitProp);
        }
        public void AddProperties(HashSet<DataUnitProp> dataUnitProps)
        {
            foreach (var dataUnit in dataUnitProps)
            {
                Props.Add(dataUnit);
            }
        }

        public bool UpdateProperty(DataUnitProp dataUnitProp)
        {
            return SetProperty(dataUnitProp.Name, dataUnitProp);
        }
        protected bool SetProperty(string name, DataUnitProp dataUnitProp)
        {
            var propToUpdate = GetProperty(name);
            if (propToUpdate != null)
            {
                bool removed = Props.Remove(propToUpdate);
                bool added = Props.Add(dataUnitProp);
                if (!added)
                {
                    Props.Add(propToUpdate);
                }
                return removed && added;
            }
            return false;
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
    }
}