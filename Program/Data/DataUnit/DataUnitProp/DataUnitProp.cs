namespace Program
{
    public abstract class DataUnitProp : IComparable
    {
        public string Name { get; }
        public object Value { get; }
        public DataType Type { get; }

        public DataUnitProp(string name, object value, DataType type)
        {
            Name = name;
            Value = value;
            Type = type;
        }
        protected bool Equals(DataUnitProp other)
        {
            return Name == other.Name;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DataUnitProp) obj);
        }
        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public int CompareTo(object? obj)
        {
            if (obj != null)
            {
                var val = (DataUnitProp) obj;
                return Name.CompareTo(val.Name);
            }
            else
            {
                return -1;
            }
        }
    }
}