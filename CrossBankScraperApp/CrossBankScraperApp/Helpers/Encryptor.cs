using System.Security.Cryptography;
using System.Text;

namespace CrossBankScraperApp.Helpers
{
    public class Encryptor
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

        public static byte[] Encryptdata(byte[] bytesToEncrypt, string md5Key, string iv)
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            DefaultCryptoProperties(cryptoProvider, md5Key, iv);

            using (var cryptoTransform = cryptoProvider.CreateEncryptor(cryptoProvider.Key, cryptoProvider.IV))
            {
                return cryptoTransform
                    .TransformFinalBlock(bytesToEncrypt, 0, bytesToEncrypt.Length);
            }
        }

        public static byte[] Decryptdata(byte[] bytesToDecrypt, string md5Key, string iv)
        {
            var cryptoProvider = new AesCryptoServiceProvider();
            DefaultCryptoProperties(cryptoProvider, md5Key, iv);

            using (var cryptoTransform = cryptoProvider.CreateDecryptor(cryptoProvider.Key, cryptoProvider.IV))
            {
                return cryptoTransform
                    .TransformFinalBlock(bytesToDecrypt, 0, bytesToDecrypt.Length);
            }   
        }

        private static void DefaultCryptoProperties(AesCryptoServiceProvider cryptoProvider, string md5Key, string iv)
        {
            cryptoProvider.BlockSize = 128;
            cryptoProvider.KeySize = 128;
            cryptoProvider.Key = Encoding.UTF8.GetBytes(md5Key);
            cryptoProvider.IV = Encoding.UTF8.GetBytes(iv);
            cryptoProvider.Padding = PaddingMode.PKCS7;
            cryptoProvider.Mode = CipherMode.CBC;
        }
    }
}