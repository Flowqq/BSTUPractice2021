using System.Collections.Generic;

namespace Program.userInterface
{
    public interface IDataUnitFileInterface
    {
        void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath,
            string divideId);
        List<DataUnit> LoadDataUnitsFromFile(string filepath);
        void SaveDataUnit(string filepath, DataUnit dataUnit);
        void DeleteDataUnit(string filepath, string dataUnitId);
    }
}