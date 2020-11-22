using System.Security.Cryptography;
using System.Text;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static string SqlString(this string value)
        {
            return $"'{value.Replace("'", "")}'";
        }

        public static bool ToBool(this string value)
        {
            if (value == "1") return true;
            return false;
        }

        public static string EncryptString(this string value)
        {
            string result = "";
            MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();
            byte[] bytesToHash = Encoding.ASCII.GetBytes(value);
            bytesToHash = cryptoService.ComputeHash(bytesToHash);
            foreach (byte streamByte in bytesToHash)
            {
                result += streamByte.ToString("x2");
            }
            return result;
        }
    }
}