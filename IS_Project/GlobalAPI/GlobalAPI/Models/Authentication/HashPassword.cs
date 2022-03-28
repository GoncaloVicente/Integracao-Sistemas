using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GlobalAPI.Models.Authentication
{
    public class HashPassword
    {
        public static string toHash (string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] hash = null;
            string passwordHash = "";

            using (SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider())
            {
                hash = sha256.ComputeHash(data);
                passwordHash = Convert.ToBase64String(hash);
            }

            return passwordHash;
        }
    }
}