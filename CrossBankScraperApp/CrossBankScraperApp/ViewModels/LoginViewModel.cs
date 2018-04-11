using BankScraper;
using BankScraper.Enums;
using CrossBankScraperApp.Helpers;
using CrossBankScraperApp.Views;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossBankScraperApp.ViewModels
{
    public sealed class LoginViewModel : BaseViewModel
    {
        private string _userLogin = string.Empty;
        private string _userPassword = string.Empty;
        private readonly Scraper _bankScraper;

        public string UserLogin
        {
            get { return _userLogin; }
            set { SetProperty(ref _userLogin, value); }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set { SetProperty(ref _userPassword, value); }
        }

        public ICommand DoLoginCommand { get; private set; }

        public LoginViewModel()
        {
            _bankScraper = new Scraper(BankFlag.Intermedium);
            DoLoginCommand = new Command(async () => await DoLoginCommandExecute());
        }

        private async Task DoLoginCommandExecute()
        {
            IsBusy = true;

            if (string.IsNullOrEmpty(_userLogin) || string.IsNullOrEmpty(_userPassword))
            {
                IsBusy = false;
                return;
            }

            var IsLogged = await _bankScraper.LoginAsync(UserLogin, UserPassword);

            if (IsLogged)
            {
                var userDetails = await _bankScraper.GetUserDetailsAsync();
                var userViewModel = new UserViewModel(userDetails);
                await NavigationHelper.PushAsync(new DashBoardPage(userViewModel));
            }

            IsBusy = false;
        }
    }
}
