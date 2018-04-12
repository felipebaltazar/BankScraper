using CrossBankScraperApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossBankScraperApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            BindingContext = new LoginViewModel();
		}
	}
}