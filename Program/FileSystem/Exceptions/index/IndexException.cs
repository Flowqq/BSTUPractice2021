using System;

namespace Program.Exceptions
{
    public class IndexException : Exception
    {
        public IndexException(string? message) : base(message)
        {
        }
    }
}