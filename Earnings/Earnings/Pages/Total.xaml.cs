using Earnings.Models;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Earnings.Pages
{
	public partial class Total : ContentPage
	{
		public static int e = 0;
		public static int ex = 0;
		public static int a = 0;
		public static int cur = 0;
		public Total()
		{
			InitializeComponent();
			InitText();
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			InitText();
		}
		private void InitText()
		{
			earned.Text = "Razem zarobione: " + e + " zł";
			spent.Text = "Razem wydane: " + ex + " zł";
			addons.Text = "Razem z dodatków: " + a + " zł";
			cur = e - ex + a;
			current.Text = "Saldo: " + cur + " zł";
		}
	}
}