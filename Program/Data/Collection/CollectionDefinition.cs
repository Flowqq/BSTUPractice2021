namespace Program
{
    public class CollectionDefinition
    {
        public string Id { get; }
        public string Name { get; }
        public int DataUnitsCount { get; }

        public CollectionDefinition(string id, string name, int dataUnitsCount)
        {
            Id = id;
            Name = name;
            DataUnitsCount = dataUnitsCount;
        }

        public Collection ToEmptyCollection()
        {
            return new Collection(Id, Name);
        }
    }
}