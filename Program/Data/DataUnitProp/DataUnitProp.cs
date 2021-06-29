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
    }
}