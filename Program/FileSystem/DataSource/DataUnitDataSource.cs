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
        public void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath,
            string divideId)
        {
            var dataUnits = LoadDataUnitsFromFile(oldIndexFilepath);
            var loverUnits = dataUnits.Where(unit => String.Compare(unit.Id, divideId, StringComparison.Ordinal) < 0);
            var upperUnits = dataUnits.Where(unit => String.Compare(unit.Id, divideId, StringComparison.Ordinal) >= 0);

            RewriteDataUnitsToFile(newLeftIndexPath, new List<DataUnit>(loverUnits));
            RewriteDataUnitsToFile(newRightIndexPath, new List<DataUnit>(upperUnits));
            DirUtils.DeleteFile(oldIndexFilepath);
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
                    for (int i = 0; i < unitsCount; i++)
                    {
                        var dataUnit = DataUnit.Deserialize(stream);
                        dataList.Add(dataUnit);
                    }
                }

                return dataList;
            }

            throw new Exception($"No file {filepath} found for loading data units!");
        }

        public void SaveDataUnit(string filepath, DataUnit dataUnit)
        {
            var dataUnits = LoadDataUnitsFromFile(filepath);
            var dataUnitToSave = dataUnits.Find(unit => unit.Id == dataUnit.Id);
            if (dataUnitToSave != null)
            {
                dataUnits.Remove(dataUnitToSave);
                dataUnits.Add(dataUnit);
                RewriteDataUnitsToFile(filepath, dataUnits);
            }
            else
            {
                AppendDataUnitToFile(filepath, dataUnit);
            }
        }

        public void DeleteDataUnit(string filepath, string dataUnitId)
        {
            var dataUnits = LoadDataUnitsFromFile(filepath);
            var dataUnitToDelete = dataUnits.Find(unit => unit.Id == dataUnitId);
            if (dataUnitToDelete != null)
            {
                dataUnits.Remove(dataUnitToDelete);
                RewriteDataUnitsToFile(filepath, dataUnits);
            }
        }

        protected void AppendDataUnitToFile(string filepath, DataUnit dataUnit)
        {
            using (var fileStream = new FileStream(filepath, FileMode.Open))
            {
                var bytes = new List<byte>();
                bytes.AddRange(dataUnit.Serialize());
                var unitsCount = SerializeUtils.ReadNextInt(fileStream);
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