using System.Threading.Tasks;
using BankScraper.Enums;
using BankScraper.Interfaces;
using BankScraper.Models;

namespace BankScraper.Banks
{
    internal class Itau : IBank
    {
        public BankFlag Flag { get { return BankFlag.ItauUnibanco; } }

        public async Task<UserDetails> GetUserDetailsAsync()
        {
            //TODO
            throw new System.NotImplementedException();
        }

        public async Task<bool> LoginAsync(string userOrAccount, string password)
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }
}
