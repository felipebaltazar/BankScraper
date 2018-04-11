using CrossBankScraperApp.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossBankScraperApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DashBoardPage : ContentPage
	{
		public DashBoardPage (UserViewModel userViewModel)
		{
			InitializeComponent ();
            BindingContext = new DashBoardViewModel(userViewModel);
		}
	}
}