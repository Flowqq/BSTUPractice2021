using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Program.Utils;

namespace Program
{
    public class IdIndex
    {
        public string MaxId { get; }
        public string MinId { get; }
        public int MaxElements { get; }

        public IdIndex Left { get; protected set; }
        public IdIndex Right { get; protected set; }
        public HashSet<string> IdList { get; }
        public string FileName { get; }
        public CollectionDefinition ColDef { get; }

        public bool IsLeaf
        {
            get => Left == null && Right == null;
        }

        public IdIndex(IdIndex left, IdIndex right, string name, CollectionDefinition colDef)
        {
            Left = left;
            Right = right;
            MaxId = right.MaxId;
            MinId = left.MinId;
            FileName = name;
            ColDef = colDef;
        }

        public IdIndex(HashSet<string> idList, string minId, string maxId, int maxElements, string fileName, CollectionDefinition colDef)
        {
            MaxId = maxId;
            MinId = minId;
            IdList = idList;
            MaxElements = maxElements;
            FileName = fileName;
            ColDef = colDef;
            Left = null;
            Right = null;
        }
        public IdIndex(CollectionDefinition colDef, int maxElements = 2)
        {
            var maxChar = Convert.ToChar(121);
            var minChar = Convert.ToChar(48);
            var maxId = new char[32];
            var minId = new char[32];
            for (int i = 0; i < maxId.Length; i++)
            {
                maxId[i] = maxChar;
                minId[i] = minChar;
            }
            MaxId = new string(maxId);
            MinId = new string(minId);
            IdList = new HashSet<string>();
            MaxElements = maxElements;
            FileName = colDef.Name;
            ColDef = colDef;
            Left = null;
            Right = null;
        }

        public string GetMaxRealId()
        {
            if (IsLeaf)
            {
                return IdList.Max();
            }
            throw new Exception("Can't get real max Id - index isn't leaf!");
        }

        public string GetMinRealId()
        {
            if (IsLeaf)
            {
                return IdList.Min();
            }
            throw new Exception("Can't get real min Id - index isn't leaf!");
        }

        public string GetFilepath()
        {
            if (IsLeaf)
            {
                return PathUtils.GetCollectionDataFilepathByIndex(this);
            }
            throw new Exception("Can't get index filepath - index isn't leaf!");
        }
        public List<string> GetAllIndexesFilePaths()
        {
            var filepathsList = new List<string>();
            if (IsLeaf)
            {
                return new List<string>() {GetFilepath()};
            }
            else
            {
                filepathsList.AddRange(Left.GetAllIndexesFilePaths());
                filepathsList.AddRange(Right.GetAllIndexesFilePaths());
            }
            return filepathsList;
        }
        public string FindIndexFilepathByUnitId(string dataUnitId)
        {
            var isInRange = IsInRange(dataUnitId);
            if (isInRange && IsLeaf)
            {
                return GetFilepath();
            }
            else if (isInRange && !IsLeaf)
            {
                if (Left.IsInRange(dataUnitId))
                {
                    return Left.FindIndexFilepathByUnitId(dataUnitId);
                }
                return Right.FindIndexFilepathByUnitId(dataUnitId);
            }
            throw new Exception($"No DataUnit with id {dataUnitId} found in index!");
        }

        public bool IsInRange(string id)
        {
            return String.Compare(id, MaxId, StringComparison.Ordinal) < 0 
                   && String.Compare(id, MinId, StringComparison.Ordinal) >= 0;
        }

        public void Divide(string midId)
        {
            var loverIds = IdList.Where(id => String.Compare(id, midId, StringComparison.Ordinal) < 0);
            var upperIds = IdList.Where(id => String.Compare(id, midId, StringComparison.Ordinal) >= 0);
            var leftFilName = FileName + "L";
            var rightFileName = FileName + "R";
            Left = new IdIndex(new HashSet<string>(loverIds), MinId, midId, MaxElements, leftFilName, ColDef);
            Right = new IdIndex(new HashSet<string>(upperIds), midId, MaxId, MaxElements, rightFileName, ColDef);
            IdList.Clear();
        }
        public IdIndex AddDataUnitIndex(string dataUnitId)
        {
            if (IsInRange(dataUnitId) && IdList.Count < MaxElements)
            {
                IdList.Add(dataUnitId);
                return null;
            }
            else if (IdList.Count == MaxElements)
            {
                IdList.Add(dataUnitId);
                return this;
            }
            else if (IsInRange(dataUnitId) && !IsLeaf)
            {
                if (Left.IsInRange(dataUnitId))
                {
                    return Left.AddDataUnitIndex(dataUnitId);
                }
                return Right.AddDataUnitIndex(dataUnitId);
            }
            throw new Exception($"DataUnit id {dataUnitId} isn't in index range - [{MinId}..{MaxId}]");
        }

        public IdIndex RemoveDataUnitIndex(string dataUnitId)
        {
            if (IsInRange(dataUnitId) && IsLeaf)
            {
                IdList.Remove(dataUnitId);
                return this;
            }
            else if (IsInRange(dataUnitId) && !IsLeaf)
            {
                if (Left.IsInRange(dataUnitId))
                {
                    return Left.RemoveDataUnitIndex(dataUnitId);
                }

                return Right.RemoveDataUnitIndex(dataUnitId);
            }
            return this;
        }

        public List<byte> Serialize()
        {
            var bytes = new List<byte>();
            bytes.Add(SerializeUtils.IntToByte(MaxElements));
            bytes.AddRange(SerializeUtils.StringToBytes(FileName));
            bytes.Add(SerializeUtils.BoolToByte(IsLeaf));
            bytes.AddRange(SerializeUtils.StringToBytes(MinId));
            bytes.AddRange(SerializeUtils.StringToBytes(MaxId));
            if (!IsLeaf)
            {
                bytes.AddRange(Left.Serialize());
                bytes.AddRange(Right.Serialize());
            }
            else
            {
                bytes.Add(SerializeUtils.IntToByte(IdList.Count));
                foreach (var id in IdList)
                {
                    bytes.AddRange(SerializeUtils.StringToBytes(id));
                }
            }
            return bytes;
        }

        public static IdIndex Deserialize(FileStream fileStream, CollectionDefinition colDef)
        {
            var maxElements = SerializeUtils.ReadNextInt(fileStream);
            var postfix = SerializeUtils.ReadNextString(fileStream);
            var isLeaf = SerializeUtils.ReadNextBool(fileStream);
            var minId = SerializeUtils.ReadNextString(fileStream);
            var maxId = SerializeUtils.ReadNextString(fileStream);
            if (!isLeaf)
            {
                var left = Deserialize(fileStream, colDef);
                var right = Deserialize(fileStream, colDef);
                return new IdIndex(left, right, postfix, colDef);
            }
            else
            {
                var idCount = SerializeUtils.ReadNextInt(fileStream);
                var idList = new HashSet<string>();
                for (int i = 0; i < idCount; i++)
                {
                    idList.Add(SerializeUtils.ReadNextString(fileStream));
                }
                return new IdIndex(idList, minId, maxId, maxElements, postfix, colDef);
            }
        }
    }
}