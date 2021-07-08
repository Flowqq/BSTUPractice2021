using System.Collections.Generic;

namespace Program.DataPage
{
    public class Page<T>
    {
        public List<T> PageData { get; }
        public int Items { get; }

        public Page(List<T> pageData)
        {
            Items = pageData.Count;
            PageData = pageData;
        }

        public void AddItem(T item)
        {
            PageData.Add(item);
        }
    }
}