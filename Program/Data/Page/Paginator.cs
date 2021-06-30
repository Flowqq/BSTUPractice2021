using System.Collections.Generic;

namespace Program.DataPage
{
    public class Paginator<T>
    {
        public SortedSet<T> DataPages { get; }
        public int CurrentPageNumber { get; }
        public int TotalPages { get; }
        
        public Paginator(int totalPages)
        {
            DataPages = new SortedSet<T>();
            CurrentPageNumber = 0;
            TotalPages = totalPages;
        }
    }
}