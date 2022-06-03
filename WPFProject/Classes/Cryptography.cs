using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WPFProject.Classes
{
    static class Cryptography
    {
        public static string HashingPass(string login, string password)
        {
            using (SHA512CryptoServiceProvider hashAlgorithm = new SHA512CryptoServiceProvider())
            {
                byte[] byteValue = Encoding.UTF8.GetBytes(login + password);
                byte[] hashValue = hashAlgorithm.ComputeHash(byteValue);

                string hashString = BitConverter.ToString(hashValue);

                return hashString;
            };
        }
    }
}
