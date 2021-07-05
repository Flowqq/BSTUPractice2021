using System.Collections.Generic;

namespace Program.DataPage
{
    public class DataUnitsPaginator : Paginator<DataUnitsPage>
    {
        public DataUnitsPaginator(int pageSize, List<DataUnit> dataPages) : base(pageSize)
        {
            CurrentPageNumber = 0;
            TotalPages = dataPages.Count / pageSize;
            int index = 0;
            for (int i = 0; i < TotalPages; i++)
            {
                var a = new List<int>();
                var newPage = new DataUnitsPage(new SortedSet<DataUnit>(dataPages.GetRange(index, pageSize)));
                index += pageSize;
                DataPages.Add(newPage);
            }
        }
    }
}