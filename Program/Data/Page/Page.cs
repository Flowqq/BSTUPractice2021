using System.Collections;

namespace Program.DataPage
{
    public class Page<T> where T : ICollection
    {
        public T PageData { get; }
        public int Items { get; protected set; }

        public Page(T pageData)
        {
            Items = 0;
            PageData = pageData;
        }
    }
}