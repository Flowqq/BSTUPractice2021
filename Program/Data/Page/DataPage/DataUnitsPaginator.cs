using System.Collections.Generic;

namespace Program.DataPage
{
    public class DataUnitsPaginator : Paginator<DataUnitsPage, SortedSet<DataUnit>>
    {
        public DataUnitsPaginator(int totalPages, List<DataUnitsPage> dataPages) : base(totalPages, dataPages)
        {
        }
    }
}