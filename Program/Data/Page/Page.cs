using System.Collections;

namespace Program.DataPage
{
    public class Page<T> where T : ICollection
    {
        public T PageData { get; }

        public Page(T pageData)
        {
            PageData = pageData;
        }
    }
}