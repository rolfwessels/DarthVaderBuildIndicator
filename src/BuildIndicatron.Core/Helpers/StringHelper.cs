using System;
using System.Security.Cryptography;
using System.Text;

namespace BuildIndicatron.Core.Helpers
{
    public static class StringHelper
    {
        public static string Md5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Convert.ToBase64String(result).TrimEnd('=').Replace("+", "").Replace("/", "");
            }
        } 
    }
}