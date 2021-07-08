using System.Collections.Generic;

namespace Program.DataPage
{
    public class DataUnitsPaginator : Paginator<DataUnitsPage>
    {
        public DataUnitsPaginator(int pageSize, List<DataUnit> dataPages) : base(pageSize)
        {
            CurrentPageNumber = 0;
            TotalPages = dataPages.Count / pageSize;
            var index = 0;
            for (var i = 0; i < TotalPages; i++)
            {
                var newPage = new DataUnitsPage(new List<DataUnit>(dataPages.GetRange(index, pageSize)));
                index += pageSize;
                DataPages.Add(newPage);
            }
        }

        public void AddNewItem(DataUnit dataUnit)
        {
            if (DataPages[TotalPages].Items == PageSize)
            {
                var newPage = new DataUnitsPage(new List<DataUnit>() {dataUnit});
                DataPages.Add(newPage);
            }
            else
            {
                DataPages[TotalPages].AddItem(dataUnit);
            }
        }
    }
}