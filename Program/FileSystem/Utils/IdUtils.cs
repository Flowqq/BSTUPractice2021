using System;
using System.Collections.Generic;
using System.Text;

namespace Program.FileSystem.Utils
{
    public class IdUtils
    {
        public static string GenerateId()
        {
            var idChars = new char[32];
            var rnd = new Random();
            for (var i = 0; i < 32; i++)
            {
                var nextChar = i switch
                {
                    < 8 => (char) rnd.Next('A', 'Z' + 1),
                    < 16 => (char) rnd.Next('a', 'z' + 1),
                    < 24 => (char) rnd.Next('0', '9' + 1),
                    _ => (char) rnd.Next('A', 'Z' + 1)
                };
                idChars[i] = nextChar;
            }
            return new string(idChars);
        }

        public static string GetMinObjId()
        {
            var idChars = new char[32];
            for (var i = 0; i < 32; i++)
            {
                var nextChar = i switch
                {
                    < 8 => 'A',
                    < 16 => 'a',
                    < 24 => '0',
                    _ => 'A'
                };
                idChars[i] = nextChar;
            }
            return new string(idChars);
        }

        public static string GetMaxObjId()
        {
            var idChars = new char[32];
            for (var i = 0; i < 32; i++)
            {
                var nextChar = i switch
                {
                    < 8 => 'Z',
                    < 16 => 'z',
                    < 24 => '9',
                    _ => 'Z'
                };
                idChars[i] = nextChar;
            }
            return new string(idChars);
        }
        public static string SubIds(string firstId, string secId)
        {
            var firstIdBytes = SerializeUtils.StringToBytes(firstId);
            var secIdBytes = SerializeUtils.StringToBytes(secId);
            var bytes = new List<byte>();
            for (var i = 1; i < firstIdBytes.Count; i++)
            {
                var fByte = Convert.ToInt32(firstIdBytes[i]);
                var sByte = Convert.ToInt32(secIdBytes[i]);
                bytes.Add(SerializeUtils.IntToByte((fByte + sByte) / 2));
            }
            return new UTF8Encoding().GetString(bytes.ToArray());
        }
    }
}