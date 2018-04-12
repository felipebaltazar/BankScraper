using BankScraper.Models;
using CrossBankScraperApp.Helpers;
using CrossBankScraperApp.Models;
using System;
using System.Linq.Expressions;
using System.Text;

namespace CrossBankScraperApp.Mappings
{
    public class FromUserToDTO : Mapping<UserDetails, UserDTO>
    {
        private const string IV = "HR$2pIjHR$2pIj12";
        private const string KEY = "B4NKSC84P38";

        protected override Expression<Func<UserDetails, UserDTO>> BuildProjection()
        {
            return user => new UserDTO
            {
                Name = EncryptString(user.Name),
                Account = EncryptString(user.Account),
                Agency = EncryptString(user.Agency),
                Balance = EncryptString(user.Balance)
            };
        }

        private string EncryptString(string stringToEncrypt)
        {
            var bytesString = Encoding.UTF8.GetBytes(stringToEncrypt);
            var md5Key = Encryptor.Md5Generate(KEY);

            return Encoding.UTF8.GetString(
                Encryptor.Encryptdata(bytesString, md5Key, IV));
        }
    }
}