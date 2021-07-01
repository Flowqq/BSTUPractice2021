namespace Program
{
    public class DataUnitProp
    {
        public string Name { get; }
        public object Value { get; }
        public DataType DataType { get; }
        
        public DataUnitProp(string name, object value, DataType dataType)
        {
            Name = name;
            Value = value;
            DataType = dataType;
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
    }
}