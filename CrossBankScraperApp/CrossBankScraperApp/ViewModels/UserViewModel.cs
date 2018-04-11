using BankScraper.Models;

namespace CrossBankScraperApp.ViewModels
{
    public sealed class UserViewModel : BaseViewModel
    {
        private string name = string.Empty;
        private string account = string.Empty;
        private string balance = string.Empty;

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        
        public string Account
        {
            get { return account; }
            set { SetProperty(ref account, value); }
        }

        public string Balance
        {
            get { return balance; }
            set { SetProperty(ref balance, value); }
        }

        public UserViewModel(UserDetails user)
        {
            Name = user.Name;
            Account = user.Account;
            Balance = user.Balance;
        }
    }
}
