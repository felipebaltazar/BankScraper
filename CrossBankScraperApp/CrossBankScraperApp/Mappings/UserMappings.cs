using BankScraper.Models;
using CrossBankScraperApp.Helpers;
using CrossBankScraperApp.Models;

namespace CrossBankScraperApp.Mappings
{
    public static class UserMappings
    {
        public static Mapping<UserDetails, UserDTO> FromUserToDTO =
            new FromUserToDTO();
    }
}