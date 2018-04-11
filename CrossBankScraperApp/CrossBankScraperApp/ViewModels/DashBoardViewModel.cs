namespace CrossBankScraperApp.ViewModels
{
    public sealed class DashBoardViewModel : BaseViewModel
    {
        private UserViewModel user;

        public UserViewModel User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public DashBoardViewModel(UserViewModel userViewModel)
        {
            User = userViewModel;
        }
    }
}
