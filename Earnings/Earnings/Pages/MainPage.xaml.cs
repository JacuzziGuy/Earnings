using Earnings.Pages;
using System;
using System.IO;
using Xamarin.Forms;

namespace Earnings
{
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();
			InitTabs();
			SetPickerValues();
		}
		private void InitTabs()
		{
			NavigationPage total = new NavigationPage(new Total());
			total.Title = "Saldo";
			NavigationPage earned = new NavigationPage(new Earned());
			earned.Title = "Zarobki";
			NavigationPage addons = new NavigationPage(new Addons());
			addons.Title = "Dodatki";
			NavigationPage spent = new NavigationPage(new Spent());
			spent.Title = "Wydatki";
			Children.Add(total);
			Children.Add(earned);
			Children.Add(spent);
			Children.Add(addons);
		}
		private void SetPickerValues()
		{
			try
			{
				File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/PaidValues.txt");
			}
			catch
			{
				string[] paidList = new string[] { "10zł", "12zł", "15zł", "20zł" };
				File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/PaidValues.txt", paidList);
			}
			try
			{
				File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/TimeValues.txt");
			}
			catch
			{
				string[] timeList = new string[] { "1h", "2h", "3h", "4h", "5h", "6h", "7h", "8h" };
				File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/TimeValues.txt", timeList);
			}
		}
	}
}
