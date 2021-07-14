using System;

namespace Program.Exceptions.collection
{
    public class CollectionNotFoundException : Exception
    {
        public CollectionNotFoundException(string? message) : base(message)
        {
        }

        public static CollectionNotFoundException GenerateException(string collectionId)
        {
            return new CollectionNotFoundException($"Collection with id - {collectionId} not found!");
        }
    }
}