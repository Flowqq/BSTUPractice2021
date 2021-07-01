using System.Collections;
using System.Collections.Generic;

namespace Program.DataPage
{
    public class Paginator<T, TV> where T : Page<TV> where TV : ICollection
    {
        public TV CurrentPageData { get => GetCurrentPageData(); }
        protected List<T> DataPages { get; }
        public int CurrentPageNumber { get; protected set; }
        public int TotalPages { get; }
        
        public Paginator(int totalPages, List<T> dataPages)
        {
            DataPages = dataPages;
            CurrentPageNumber = 0;
            TotalPages = totalPages;
        }
        protected TV GetCurrentPageData()
        {
            return DataPages[CurrentPageNumber].PageData;
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
    }
}