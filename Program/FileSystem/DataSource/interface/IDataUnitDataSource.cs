using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IDataUnitDataSource
    {
        void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath,
            string divideId);
        List<DataUnit> LoadDataUnitsFromFile(string filepath);
        bool SaveDataUnit(string filepath, DataUnit dataUnit);
        bool DeleteDataUnit(string filepath, string dataUnitId);
        public void UniteDataIndex(string parentIndexFilepath, string leftIndexPath, string rightIndexPath);
    }
}