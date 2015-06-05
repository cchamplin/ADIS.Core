using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Security
{
    public static class Crypto
    {
        public static byte[] CreateSalt()
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }
        public static string CreateHash(string password, string base64Salt)
        {
            // Generate a random salt
            byte[] salt = Convert.FromBase64String(base64Salt);

            return CreateHash(password, salt);
        }
        public static string CreateHash(string password, byte[] salt)
        {
           
            byte[] hash = PBKDF2(password, salt, 1000, 24);
            return Convert.ToBase64String(hash);
        }

       
        public static bool ValidatePassword(string password, string salt, string correctHash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] correctBytes = Convert.FromBase64String(correctHash);

            byte[] testHash = PBKDF2(password, saltBytes, 1000, 24);
            return SlowEquals(testHash, correctBytes);
        }

        
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

      
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
