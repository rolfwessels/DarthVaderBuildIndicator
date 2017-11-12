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

        public static string MaskInput(this string input, int charactersToShowAtEnd = 5)
        {
            return input;
//            if (string.IsNullOrEmpty(input)) return null;
//            if (input.Length < charactersToShowAtEnd)
//                charactersToShowAtEnd = input.Length;
//            var endCharacters = input.Substring(input.Length - charactersToShowAtEnd);
//            return string.Format("{0}{1}", "".PadLeft(input.Length - charactersToShowAtEnd, '*') + endCharacters
//            );
        }
    }
}