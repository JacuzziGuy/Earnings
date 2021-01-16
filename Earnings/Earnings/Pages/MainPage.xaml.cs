using Earnings.Pages;
using Xamarin.Forms;

namespace Earnings
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();
			InitTabs();
		}
		private void InitTabs()
		{
			NavigationPage total = new NavigationPage(new Total());
			total.Title = "Saldo";
			NavigationPage earned = new NavigationPage(new Earned());
			earned.Title = "Zarobki";
			NavigationPage addons = new NavigationPage(new Addons());
			addons.Title = "Dodatki";
			NavigationPage spent = new NavigationPage(new Expenses());
			spent.Title = "Wydatki";
			Children.Add(total);
			Children.Add(earned);
			Children.Add(spent);
			Children.Add(addons);
		}
	}
}
