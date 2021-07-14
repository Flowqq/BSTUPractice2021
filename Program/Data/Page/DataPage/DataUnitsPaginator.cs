using System.Collections.Generic;

namespace Program.DataPage
{
    public class DataUnitsPaginator : Paginator<DataUnitsPage>
    {
        public DataUnitsPaginator(int pageSize, List<DataUnit> dataPages) : base(pageSize)
        {
            CurrentPageNumber = 0;
            if (dataPages.Count < pageSize)
            {
                TotalPages = 1;
            }
            else
            {
                TotalPages = dataPages.Count / pageSize;
            }
            var index = 0;
            for (var i = 0; i < TotalPages; i++)
            {
                DataUnitsPage newPage = null;
                if (dataPages.Count > index + pageSize)
                {
                    newPage = new DataUnitsPage(new List<DataUnit>(dataPages.GetRange(index, pageSize)));
                }
                else
                {
                    newPage = new DataUnitsPage(new List<DataUnit>(dataPages.GetRange(index, dataPages.Count - index)));
                }
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