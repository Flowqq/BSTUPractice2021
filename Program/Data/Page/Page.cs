namespace Program.DataPage
{
    public class Page<T>
    {
        public T PageData { get; }

        public Page(T pageData)
        {
            PageData = pageData;
        }
    }
}