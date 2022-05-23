using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace BankingApp.Service
{
    public static class Hasher
    {
        public static string Cipher(this string password)
        {
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] passwordInBytes = Encoding.ASCII.GetBytes(password);
            byte[] saltInBytes = sha1.ComputeHash(passwordInBytes);

            return Convert.ToBase64String(saltInBytes);
        }
    }
}