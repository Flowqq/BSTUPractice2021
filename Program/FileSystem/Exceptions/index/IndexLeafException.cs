namespace Program.Exceptions
{
    public class IndexLeafException : IndexException
    {
        
        public IndexLeafException(string? message) : base(message)
        {
        }

        public static IndexException GenerateException(IdIndex index)
        {
            return new IndexLeafException($"Can't get requested data - index isn't leaf! Index - {index}");
        }
    }
}