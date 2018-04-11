using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrossBankScraperApp.Helpers
{
    public static class NavigationHelper
    {
        private static INavigation Navigation => CurrentPage.Navigation;        
        public static Page CurrentPage => Application.Current.MainPage;
        
        public static async Task PopAsync()
        {
            var masterDetailPage = CurrentPage as MasterDetailPage;

            if (masterDetailPage != null)
                await masterDetailPage.Detail.Navigation.PopAsync(true);
            else
                await Navigation.PopAsync(true);
        }

        public static async Task PopModalAsync()
        {
            var masterDetailPage = CurrentPage as MasterDetailPage;

            if (masterDetailPage != null)
                await masterDetailPage.Detail.Navigation.PopModalAsync(true);
            else
                await Navigation.PopModalAsync(true);
        }

        public static async Task PushModalAsync(Page page)
        {
            var masterDetailPage = CurrentPage as MasterDetailPage;

            if (masterDetailPage != null)
                await masterDetailPage.Detail.Navigation.PushModalAsync(page, true);
            else
                await Navigation.PushModalAsync(page, true);
        }

        public static async Task PushAsync(Page page)
        {
            var masterDetailPage = CurrentPage as MasterDetailPage;

            if (masterDetailPage != null)
                await masterDetailPage.Detail.Navigation.PushAsync(page);
            else
                await Navigation.PushAsync(page);
        }

        public static async Task PushAsync(int index)
        {
            var masterDetailPage = CurrentPage as MasterDetailPage;

            if (masterDetailPage != null)
                await masterDetailPage.Detail.Navigation.PushAsync(Navigation.NavigationStack[index]);
            else
                await Navigation.PushAsync(Navigation.NavigationStack[index]);
        }

        public static async Task PopToNewRootAsync(Page newRootPage)
        {
            NewRootPage(newRootPage);
            await Navigation.PopToRootAsync(true);
        }

        public static async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync(true);
        }

        public static void InsertBeforePage(Page pageToInsert, Page before) =>
            Navigation.InsertPageBefore(pageToInsert, before);

        public static void InsertBeforePage(Page PageToInsert, int beforeIndex) =>
            Navigation.InsertPageBefore(PageToInsert, Navigation.NavigationStack[beforeIndex]);

        public static void RemoveFromNavigationStack(Page pageToRemove) =>
            Navigation.RemovePage(pageToRemove);

        public static void RemoveFromNavigationStack(int index) =>
            Navigation.RemovePage(Navigation.NavigationStack[index]);

        public static void NewRootPage(Page page)
        {
            Navigation.InsertPageBefore(page, Navigation.NavigationStack[0]);
        }

        public static void ChangeBackButtonVisibility(bool visibility)
        {
            var currentPage = CurrentPage;

            while (currentPage is NavigationPage)
                currentPage = ((NavigationPage)currentPage).CurrentPage;

            NavigationPage.SetHasBackButton(currentPage, visibility);
        }
    }
}
