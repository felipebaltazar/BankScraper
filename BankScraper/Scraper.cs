using BankScraper.Enums;
using BankScraper.Helpers;
using BankScraper.Interfaces;
using BankScraper.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankScraper
{
    public sealed class Scraper
    {
        private readonly IBank _bank;

        #region Constructors

        public Scraper(BankFlag flag)
        {
            _bank = BankCollectionManager.AllBanks
                .FirstOrDefault(b => b.Flag == flag);
            
            if (_bank == null)
                throw new NullReferenceException($"Cant initialize scraper for bank {flag.ToString()}");
        }

        #endregion

        #region Public Actions

        public async Task<bool> LoginAsync(string userOrAccount, string password)
        {
            if (string.IsNullOrEmpty(userOrAccount) || string.IsNullOrEmpty(password))
                throw new NullReferenceException($"user and password can't be null or empty");

            return await _bank.LoginAsync(userOrAccount, password);
        }

        public async Task<UserDetails> GetUserDetailsAsync()
        {
            if (_bank == null)
                throw new NullReferenceException("Bank scraper is not initialized");

            return await _bank.GetUserDetailsAsync();
        }

        #endregion
    }
}