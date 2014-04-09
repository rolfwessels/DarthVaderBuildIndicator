using System.Security.Cryptography;
using System.Text;

namespace BuildIndicatron.Console
{
    public class SimpleCrypt
    {
        private static byte[] _entropy;

        public SimpleCrypt()
            : this(new byte[] {2, 3, 4, 5, 5})
        {
        }

        public SimpleCrypt(byte[] entropy)
        {
            _entropy = entropy;
        }

        public string Protect(string toEncrypt)
        {
            var encryptedData = ProtectedData.Protect(Encoding.UTF8.GetBytes(toEncrypt), _entropy,
                DataProtectionScope.CurrentUser);
            return System.Convert.ToBase64String(encryptedData);
        }

        public string Unprotect(string toDecrypt)
        {
            var dec = ProtectedData.Unprotect(System.Convert.FromBase64String(toDecrypt), _entropy,
                DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(dec);
        }
    }
}