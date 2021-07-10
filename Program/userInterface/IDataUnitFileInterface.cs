using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IDataUnitFileInterface
    {
        void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath,
            string divideId);
        List<DataUnit> LoadDataUnitsFromFile(string filepath);
        void SaveDataUnitsToFile(string filepath, List<DataUnit> dataUnits);
        void SaveDataUnit(string filepath, DataUnit dataUnit);
        public void DeleteFile(string filepath);
    }
}