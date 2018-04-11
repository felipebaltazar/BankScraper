using BankScraper.Models;
using CrossBankScraperApp.Helpers;
using CrossBankScraperApp.Models;
using System;
using System.Linq.Expressions;

namespace CrossBankScraperApp.Mappings
{
    public class FromUserToDTO : Mapping<UserDetails, UserDTO>
    {
        protected override Expression<Func<UserDetails, UserDTO>> BuildProjection()
        {
            return user => new UserDTO
            {
                Name = HashManager.Md5Generate(user.Name),
                Account = HashManager.Md5Generate(user.Account),
                Agency = HashManager.Md5Generate(user.Agency),
                Balance = HashManager.Md5Generate(user.Balance)
            };
        }
    }
}