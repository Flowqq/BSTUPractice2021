using System;

namespace Program.FileSystem.Utils
{
    public class IdUtils
    {
        public static string GenerateId()
        {
            var idChars = new char[32];
            var rnd = new Random();
            for (int j = 0; j < 8; j++)
            {
                idChars[j] = (char)rnd.Next( 'A', 'Z' + 1 );
            }
            for (int j = 8; j < 16; j++)
            {
                idChars[j] = (char)rnd.Next( 'a', 'z' + 1 );
            }
            for (int j = 16; j < 24; j++)
            {
                idChars[j] = (char)rnd.Next( '0', '9' + 1 );
            }
            for (int j = 24; j < 32; j++)
            {
                idChars[j] = (char)rnd.Next( 'A', 'Z' + 1 );
            }
            
            return new string(idChars);
        }
        public static string GetMinObjId()
        {
            var idChars = new char[32];
            for (int j = 0; j < 8; j++)
            {
                idChars[j] = 'A';
            }
            for (int j = 8; j < 16; j++)
            {
                idChars[j] = 'a';
            }
            for (int j = 16; j < 24; j++)
            {
                idChars[j] = '0';
            }
            for (int j = 24; j < 32; j++)
            {
                idChars[j] = 'A';
            }
            return new string(idChars);
        }

        public static string GetMaxObjId()
        {
            var idChars = new char[32];
            var rnd = new Random();
            for (int j = 0; j < 8; j++)
            {
                idChars[j] = 'Z';
            }
            for (int j = 8; j < 16; j++)
            {
                idChars[j] = 'z';
            }
            for (int j = 16; j < 24; j++)
            {
                idChars[j] = '9';
            }
            for (int j = 24; j < 32; j++)
            {
                idChars[j] = 'Z';
            }

            return new string(idChars);
        }
    }
}