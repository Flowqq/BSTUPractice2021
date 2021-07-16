using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Program.userInterface;
using Program.Utils;

namespace Program
{
    public class DataUnitDataSource : IDataUnitDataSource
    {
        public void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath)
        {
            var dataUnits = LoadDataUnitsFromFile(oldIndexFilepath);
            dataUnits.Sort((firstUnit, secUnit) => String.Compare(firstUnit.Id, secUnit.Id, StringComparison.Ordinal));
            var loverUnits = new List<DataUnit>();
            var upperUnits = new List<DataUnit>();
            var counter = 0;
            foreach (var unit in dataUnits)
            {
                if (counter < dataUnits.Count / 2)
                {
                    loverUnits.Add(unit);
                }
                else
                {
                    upperUnits.Add(unit);
                }
                counter++;
            }

            RewriteDataUnitsToFile(newLeftIndexPath, new List<DataUnit>(loverUnits));
            RewriteDataUnitsToFile(newRightIndexPath, new List<DataUnit>(upperUnits));
            DirUtils.DeleteFile(oldIndexFilepath);
        }

        public void UniteDataIndex(string parentIndexFilepath, string leftIndexPath, string rightIndexPath)
        {
            var leftDataUnits = LoadDataUnitsFromFile(leftIndexPath);
            var rightDataUnits = LoadDataUnitsFromFile(rightIndexPath);
            var allDataUnits = new List<DataUnit>();
            allDataUnits.AddRange(leftDataUnits);
            allDataUnits.AddRange(rightDataUnits);
            RewriteDataUnitsToFile(parentIndexFilepath, allDataUnits);
            DirUtils.DeleteFile(leftIndexPath);
            DirUtils.DeleteFile(rightIndexPath);
        }
        public List<DataUnit> LoadDataUnitsFromFile(string filepath)
        {
            var fileExists = File.Exists(filepath);
            if (fileExists)
            {
                var dataList = new List<DataUnit>();
                using (var stream = new FileStream(filepath, FileMode.Open))
                {
                    var unitsCount = SerializeUtils.ReadNextInt(stream);
                    for (var i = 0; i < unitsCount; i++)
                    {
                        var dataUnit = DataUnit.Deserialize(stream);
                        dataList.Add(dataUnit);
                    }
                }
                return dataList;
            }
            throw new FileNotFoundException($"No file {filepath} found for loading data units!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="dataUnit"></param>
        /// <returns>Запись сохранена как новая</returns>
        public bool SaveDataUnit(string filepath, DataUnit dataUnit)
        {
            var dataUnits = LoadDataUnitsFromFile(filepath);
            var dataUnitToSave = dataUnits.Find(unit => unit.Id == dataUnit.Id);
            if (dataUnitToSave != null)
            {
                dataUnits.Remove(dataUnitToSave);
                dataUnits.Add(dataUnit);
                RewriteDataUnitsToFile(filepath, dataUnits);
                return false;
            }
            else
            {
                AppendDataUnitToFile(filepath, dataUnit);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="dataUnitId"></param>
        /// <returns>Запись была удалена</returns>
        public bool DeleteDataUnit(string filepath, string dataUnitId)
        {
            var dataUnits = LoadDataUnitsFromFile(filepath);
            var dataUnitToDelete = dataUnits.Find(unit => unit.Id == dataUnitId);
            if (dataUnitToDelete != null)
            {
                dataUnits.Remove(dataUnitToDelete);
                RewriteDataUnitsToFile(filepath, dataUnits);
                return true;
            }
            return false;
        }
        protected void AppendDataUnitToFile(string filepath, DataUnit dataUnit)
        {
            using (var fileStream = new FileStream(filepath, FileMode.Open))
            {
                var bytes = new List<byte>();
                bytes.AddRange(dataUnit.Serialize());
                var unitsCount = SerializeUtils.ReadNextInt(fileStream);
                if (unitsCount == -1)
                {
                    unitsCount = 0;
                }
                unitsCount++;
                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.WriteByte(SerializeUtils.IntToByte(unitsCount));
                fileStream.Seek(0, SeekOrigin.End);
                fileStream.Write(bytes.ToArray(), 0, bytes.Count);
            }
        }

        protected void RewriteDataUnitsToFile(string filepath, List<DataUnit> dataUnits)
        {
            var unicUnits = dataUnits.GroupBy(x => x)
                .Where(x => x.Count() == 1)
                .Select(x => x.Key)
                .ToList();
            var fileExists = File.Exists(filepath);
            if (!fileExists)
            {
                DirUtils.CreateDirsForFile(filepath);
            }
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                var bytes = new List<byte>();
                bytes.Add(SerializeUtils.IntToByte(unicUnits.Count));
                foreach (var dataUnit in unicUnits)
                {
                    bytes.AddRange(dataUnit.Serialize());
                }
                stream.Write(bytes.ToArray(), 0, bytes.Count);
            }
        }
    }
}