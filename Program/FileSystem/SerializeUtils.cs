using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Program
{
    public class SerializeUtils
    {
        public static List<byte> StringToBytes(string str)
        {
            var bytes = new List<byte>();
            var strBytes = new UTF8Encoding().GetBytes(str);
            var strLength = strBytes.Length;
            bytes.Add(IntToByte(strLength));
            bytes.AddRange(strBytes.ToArray());
            return bytes;
        }
        public static string ReadNextString(FileStream fileStream)
        {
            var strLength = ReadNextInt(fileStream);
            var strBuffer = new byte[strLength];
            fileStream.Read(strBuffer, 0, strLength);
            return new UTF8Encoding().GetString(strBuffer);
        }
        public static List<byte> DoubleToBytes(double val)
        {
            var bytes = new List<byte>();
            var doubleBytes = new List<byte>(BitConverter.GetBytes(val));
            var doubleLen = doubleBytes.Count;
            bytes.Add(IntToByte(doubleLen));
            bytes.AddRange(doubleBytes);;
            return bytes;
        }
        public static double ReadNextDouble(FileStream fileStream)
        {
            var doubleLength = ReadNextInt(fileStream);
            var doubleBuffer = new byte[doubleLength];
            fileStream.Read(doubleBuffer, 0, doubleLength);
            return BitConverter.ToDouble(doubleBuffer, 0);
        }

        public static byte IntToByte(int val)
        {
            return Convert.ToByte(val);
        }

        public static int ReadNextInt(FileStream fileStream)
        {
            return fileStream.ReadByte();
        }

        public static CollectionDefinition ReadCollectionDefinition(FileStream fs)
        {
            var collectionId = ReadNextString(fs);
            var collectionName = ReadNextString(fs);
            var dataUnitsCount = ReadNextInt(fs);
            return new CollectionDefinition(collectionId, collectionName, dataUnitsCount);
        }
        public static List<byte> ArrayToBytes(ICollection array, Func<object, List<byte>> serializeFunc)
        {
            var elementsCount = array.Count;
            var bytes = new List<byte>(elementsCount) {IntToByte(elementsCount)};
            foreach (var val in array)
            {
                bytes.AddRange(serializeFunc(val));
            }
            return bytes;
        }

        public static ICollection ReadNextArray(FileStream fileStream, Func<FileStream, object> deserializeFunc)
        {
            var resValue = new List<object>();
            var elementsCount = ReadNextInt(fileStream);
            for (var i = 0; i < elementsCount; i++)
            {
                resValue.Add(deserializeFunc(fileStream));
            }
            return resValue;
        }

        public static byte BoolToByte(bool val)
        {
            return Convert.ToByte(val);
        }

        public static bool ReadNextBool(FileStream fileStream)
        {
            return Convert.ToBoolean(fileStream.ReadByte());
        }
    }
}