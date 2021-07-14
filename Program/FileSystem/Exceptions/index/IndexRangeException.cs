namespace Program.Exceptions
{
    public class IndexRangeException : IndexException
    {
        
        public IndexRangeException(string? message) : base(message)
        {
        }

        public static IndexRangeException GenerateException(IdIndex index, string id)
        {
            return new IndexRangeException($"DataUnit with id - {id} can't be placed in index! Index range - [{index.MinId}..{index.MaxId}]");
        }
    }
}