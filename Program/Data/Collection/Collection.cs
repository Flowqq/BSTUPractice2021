using System.Collections.Generic;
using System.Linq;
using Program.DataPage;

namespace Program
{
    public class Collection
    {
        public string Id { get; }
        public string Name { get; }
        public List<DataUnit> DataUnits { get; }

        public CollectionDefinition Definition
        {
            get => new CollectionDefinition(Id, Name, DataUnits.Count);
        }
        public DataUnitsPaginator GetDataPaginator(int pageSize)
        {
            return new DataUnitsPaginator(pageSize, new List<DataUnit>(DataUnits));
        }

        public Collection(string id, string name)
        {
            Id = id;
            Name = name;
            DataUnits = new List<DataUnit>();
        }

        public Collection(string id, string name, List<DataUnit> dataUnits)
        {
            Id = id;
            Name = name;
            DataUnits = dataUnits;
        }

        public void AddDataUnit(DataUnit dataUnit)
        {
             DataUnits.Add(dataUnit);
        }

        public void RemoveDataUnit(string dataUnitId)
        {
            var dataUnitToRemove = FindDataUnitById(dataUnitId);
            if (dataUnitToRemove != null)
            {
                DataUnits.Remove(dataUnitToRemove);
            }
        }

        public bool UpdateDataUnit(string dataUnitId, SortedSet<DataUnitProp> updatedProps)
        {
            var dataUnitToUpdate = FindDataUnitById(dataUnitId);
            if (dataUnitToUpdate != null)
            {
                dataUnitToUpdate.Update(updatedProps);
                return true;
            }
            return false;
        }

        public List<DataUnit> SearchDataUnits(List<DataUnitProp> searchProps)
        {
            var resultSet = new List<DataUnit>();
            foreach (var dataUnit in DataUnits)
            {
                var matches = searchProps.All(searchField => dataUnit.GetProperty(searchField.Name).Value.Equals(searchField.Value));
                if (matches)
                {
                    resultSet.Add(dataUnit);
                }
            }
            return resultSet;
        }

        public DataUnit FindDataUnitById(string dataUnitId)
        {
            return DataUnits.FirstOrDefault(dataUnit => dataUnit.Id == dataUnitId);
        }
        protected bool Equals(Collection other)
        {
            return Id == other.Id;
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