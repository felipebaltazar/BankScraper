using System.Security.Cryptography;
using System.Text;

namespace CrossBankScraperApp.Helpers
{
    public class HashManager
    {
        public static string Md5Generate(string input)
        {
            var hash = MD5.Create();
            var hashData = hash.ComputeHash(Encoding.UTF8.GetBytes(input));            
            var stringBuilder = new StringBuilder();
            
            for (int i = 0; i < hashData.Length; i++)
                stringBuilder.Append(hashData[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
