using System.Collections;
using System.Collections.Generic;

namespace Program.DataPage
{
    public abstract class Paginator<T> : IEnumerator<T>
    {
        public T Current
        {
            get => GetCurrentPageData();
        }
        object IEnumerator.Current => Current;
        public List<T> DataPages { get; set; }
        public int CurrentPageNumber { get; protected set; }
        public int TotalPages { get; set; }
        public int PageSize { get; }

        public Paginator(int pageSize)
        
        {
            DataPages = new List<T>();
            CurrentPageNumber = -1;
            PageSize = pageSize;
            TotalPages = 0;
        }

        protected T GetCurrentPageData()
        {
            return DataPages[CurrentPageNumber];
        }
        public bool MoveNext()
        {
            if (CurrentPageNumber < TotalPages)
            {
                CurrentPageNumber++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            CurrentPageNumber = -1;
        }

        public void Dispose()
        {
            CurrentPageNumber = -1;
            DataPages = new List<T>(0);
        }
    }
}