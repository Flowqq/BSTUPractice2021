using System.Collections.Generic;

namespace Program.DataPage
{
    public abstract class Page<T>
    {
        public List<T> _pageData = new List<T>();

        public List<T> PageData
        {
            get => GetData();
        }

        public int Items { get; }

        public Page(List<T> pageData)
        {
            Items = pageData.Count;
            _pageData = pageData;
        }

        public void AddItem(T item)
        {
            PageData.Add(item);
        }

        public virtual List<T> GetData()
        {
            return _pageData;
        }
    }
}