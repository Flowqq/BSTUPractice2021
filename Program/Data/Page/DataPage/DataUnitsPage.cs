using System.Collections.Generic;

namespace Program.DataPage
{
    public class DataUnitsPage : Page<SortedSet<DataUnit>>
    {
        public DataUnitsPage(SortedSet<DataUnit> pageData) : base(pageData)
        {
            Items = pageData.Count;
        }
    }
}