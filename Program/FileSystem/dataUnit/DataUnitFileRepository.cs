using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Program.userInterface;

namespace Program
{
    public class DataUnitFileRepository : IDataUnitFileInterface
    {
        public void DivideIndexDataByTwo(string oldIndexFilepath, string newLeftIndexPath, string newRightIndexPath, string divideId)
        {
            var dataUnits = LoadDataUnitsFromFile(oldIndexFilepath);
            var loverUnits = dataUnits.Where(unit => String.Compare(unit.Id, divideId, StringComparison.Ordinal) < 0);
            var upperUnits = dataUnits.Where(unit => String.Compare(unit.Id, divideId, StringComparison.Ordinal) >= 0);

            SaveDataUnitsToFile(newLeftIndexPath, new List<DataUnit>(loverUnits));
            SaveDataUnitsToFile(newRightIndexPath, new List<DataUnit>(upperUnits));
            DeleteFile(oldIndexFilepath);
            
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

        public void SaveDataUnitsToFile(string filepath, List<DataUnit> dataUnits)
        {
            var fileExists = File.Exists(filepath);
            if (fileExists)
            {
                using (var fileStream = new FileStream(filepath, FileMode.Open))
                {
                    var bytes = new List<byte>();
                    foreach (var dataUnit in dataUnits)
                    {
                        bytes.AddRange(dataUnit.Serialize());
                    }
                    var unitsCount = SerializeUtils.ReadNextInt(fileStream);
                    unitsCount++;
                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.WriteByte(SerializeUtils.IntToByte(unitsCount));
                    fileStream.Seek(0, SeekOrigin.End);
                    fileStream.Write(bytes.ToArray(), 0, bytes.Count);
                }
            }
            else
            {
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    var bytes = new List<byte>();
                    bytes.Add(SerializeUtils.IntToByte(dataUnits.Count));
                    foreach (var dataUnit in dataUnits)
                    {
                        bytes.AddRange(dataUnit.Serialize());
                    }
                    stream.Write(bytes.ToArray(), 0, bytes.Count);
                }
            }
        }
        public void SaveDataUnit(string filepath, DataUnit dataUnit)
        {
            SaveDataUnitsToFile(filepath, new List<DataUnit>() {dataUnit});
        }

        public void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            else
            {
                throw new Exception($"File to delete - {filepath} not exists!");
            }
        }
    }
}