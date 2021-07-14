namespace Program.Exceptions
{
    public class IndexNotFoundException : IndexException
    {
        
        public IndexNotFoundException(string? message) : base(message)
        {
        }

        public static IndexNotFoundException GenerateException(string collectionId)
        {
            return new IndexNotFoundException($"Index for collection with id {collectionId} not found!");
        }
    }
}