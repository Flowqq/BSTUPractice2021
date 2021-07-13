using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Program.FileSystem.Utils;
using Program.Utils;

namespace Program
{
    public class IdIndex
    {
        public int ElementsCount
        {
            get
            {
                if (IsLeaf)
                {
                    return IdList.Count;
                }
                return Left.ElementsCount + Right.ElementsCount;
            }
        }
        public bool IsLeaf
        {
            get => Left == null && Right == null;
        }
        public string MaxId { get; }
        public string MinId { get; }
        public int MaxElements { get; }
        public IdIndex Left { get; protected set; }
        public IdIndex Right { get; protected set; }
        public HashSet<string> IdList { get; }
        public string FileName { get; }
        public string CollectionId { get; }

        public IdIndex(IdIndex left, IdIndex right, string fileName, string collectionId)
        {
            Left = left;
            Right = right;
            MaxId = right.MaxId;
            MinId = left.MinId;
            FileName = fileName;
            CollectionId = collectionId;
        }

        public IdIndex(HashSet<string> idList, string minId, string maxId, int maxElements, string fileName,
            string collectionId)
        {
            MaxId = maxId;
            MinId = minId;
            IdList = idList;
            MaxElements = maxElements;
            FileName = fileName;
            CollectionId = collectionId;
            Left = null;
            Right = null;
        }
        public IdIndex(CollectionDefinition colDef, int maxElements = 2)
        {
            MaxId = IdUtils.GetMaxObjId();
            MinId = IdUtils.GetMinObjId();
            IdList = new HashSet<string>();
            MaxElements = maxElements;
            FileName = colDef.Name;
            CollectionId = colDef.Id;
            Left = null;
            Right = null;
        }

        public string GetRealMaxId()
        {
            if (IsLeaf)
            {
                return IdList.Max();
            }
            throw new Exception("Can't get real max Id - index isn't leaf!");
        }

        public string GetRealMinId()
        {
            if (IsLeaf)
            {
                return IdList.Min();
            }
            throw new Exception("Can't get real min Id - index isn't leaf!");
        }

        public string GetFilepath()
        {
            if (!IsLeaf)
            {
                Console.Write("WARNING! You get index filepath but this index isn't leaf!");
                return PathUtils.GetCollectionDataFilepathByIndex(this);
            }
            return PathUtils.GetCollectionDataFilepathByIndex(this);
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
            throw new Exception($"DataUnit id {dataUnitId} isn't in index range - [{MinId}..{MaxId}]");
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
            Left = new IdIndex(new HashSet<string>(loverIds), MinId, midId, MaxElements, leftFilName, CollectionId);
            Right = new IdIndex(new HashSet<string>(upperIds), midId, MaxId, MaxElements, rightFileName, CollectionId);
            IdList.Clear();
        }

        public void Unite()
        {
            IdList.Clear();
            IdList.UnionWith(Left.IdList);
            IdList.UnionWith(Right.IdList);
            Left = null;
            Right = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="dataUnitId"></param>
        /// <returns>Индекс, кторый нужно разделить на 2</returns>
        /// <exception cref="Exception"></exception>
        public IdIndex AddDataUnitIndex(string dataUnitId)
        {
            if (IsInRange(dataUnitId))
            {
                if (ElementsCount < MaxElements)
                {
                    IdList.Add(dataUnitId);
                    return null;
                }
                else if (IdList.Count == MaxElements)
                {
                    IdList.Add(dataUnitId);
                    return this;
                }
                else if (!IsLeaf)
                {
                    if (Left.IsInRange(dataUnitId))
                    {
                        return Left.AddDataUnitIndex(dataUnitId);
                    }
                    return Right.AddDataUnitIndex(dataUnitId);
                }
            }
            throw new Exception($"DataUnit id {dataUnitId} isn't in index range - [{MinId}..{MaxId}]");
        }

        /// <summary>
        /// </summary>
        /// <param name="dataUnitId"></param>
        /// <returns>Индекс, который нужно объеденить</returns>
        public IdIndex RemoveDataUnitIndex(string dataUnitId)
        {
            if (IsInRange(dataUnitId))
            {
                if (IsLeaf)
                {
                    IdList.Remove(dataUnitId);
                    return null;
                }
                else if (Left.ElementsCount == 1 || Right.ElementsCount == 1)
                {
                    if (Left.IsInRange(dataUnitId))
                    {
                        Left.RemoveDataUnitIndex(dataUnitId);
                    }
                    else
                    {
                        Right.RemoveDataUnitIndex(dataUnitId);
                    }
                    return this;
                }
                else if(!IsLeaf)
                {
                    if (Left.IsInRange(dataUnitId))
                    {
                        return Left.RemoveDataUnitIndex(dataUnitId);
                    }
                    return Right.RemoveDataUnitIndex(dataUnitId);
                }
            }
            throw new Exception($"DataUnit id {dataUnitId} isn't in index range - [{MinId}..{MaxId}]");
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
                return new IdIndex(left, right, postfix, colDef.Id);
            }
            else
            {
                var idCount = SerializeUtils.ReadNextInt(fileStream);
                var idList = new HashSet<string>();
                for (int i = 0; i < idCount; i++)
                {
                    idList.Add(SerializeUtils.ReadNextString(fileStream));
                }
                return new IdIndex(idList, minId, maxId, maxElements, postfix, colDef.Id);
            }
        }
    }
}