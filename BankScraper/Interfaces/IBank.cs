using BankScraper.Enums;
using BankScraper.Models;
using System.Threading.Tasks;

namespace BankScraper.Interfaces
{
    public interface IBank
    {
        BankFlag Flag { get; }

        Task<bool> LoginAsync(string userOrAccount, string password);

        Task<UserDetails> GetUserDetailsAsync();
    }
}
