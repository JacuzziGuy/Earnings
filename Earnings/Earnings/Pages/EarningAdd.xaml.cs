using SQLite;
using Earnings.Models;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Earnings.Pages
{
	public partial class EarningAdd : PopupPage
	{
		int _paid = 0, _time = 0, _day = DateTime.Now.Day, _month = DateTime.Now.Month, _year = DateTime.Now.Year;
		ObservableCollection<EarningsModel> _earns = new ObservableCollection<EarningsModel>();
		SQLiteConnection db = DBModel.DBPath();
		public EarningAdd(ObservableCollection<EarningsModel> earns)
		{
			InitializeComponent();
			_earns = earns;
			InitPage();
		}
		private void InitPage()
		{
			day.SelectedItem = _day.ToString();
			month.SelectedItem = _month.ToString();
			year.SelectedItem = _year.ToString();
		}
		private void AddClicked(object sender, EventArgs e)
		{
			if (DateValid())
			{
				int cash = _paid * _time;
				EarningsModel earn = new EarningsModel { Cash = cash, IsVisible = false, Day = _day, Month = _month, Year = _year };
				Total.e += cash;
				db.Insert(earn);
				_earns.Add(earn);
				PopupNavigation.Instance.PopAsync();
			}
		}
		private void CancelClicked(object sender, EventArgs e)
		{
			PopupNavigation.Instance.PopAsync();
		}
		private void paid_SelectedIndexChanged(object sender, EventArgs e)
		{
			_paid = int.Parse(paid.SelectedItem.ToString().Replace("zł", ""));
			time.Focus();
		}
		private void time_SelectedIndexChanged(object sender, EventArgs e)
		{
			_time = int.Parse(time.SelectedItem.ToString().Replace("h", ""));
		}
		private void day_SelectedIndexChanged(object sender, EventArgs e)
		{
			_day = int.Parse(day.SelectedItem.ToString());
		}
		private void month_SelectedIndexChanged(object sender, EventArgs e)
		{
			_month = int.Parse(month.SelectedItem.ToString());
		}
		private void year_SelectedIndexChanged(object sender, EventArgs e)
		{
			_year = int.Parse(year.SelectedItem.ToString());
		}
		private bool DateValid()
		{
			if (_month == 2 && _day > 28 || _month == 4 && _day == 31 || _month == 6 && _day == 31 || _month == 9 && _day == 31 || _month == 11 && _day == 31)
			{
				if (_month == 2)
				{
					if (_year % 4 != 0)
					{
						DisplayAlert("UWAGA!", "Luty w roku " + _year + " ma tylko 28 dni!", "OK");
						return false;
					}
					else if (_day > 29 && _year % 4 == 0)
					{
						DisplayAlert("UWAGA!", "Luty w roku " + _year + " ma tylko 29 dni!", "OK");
						return false;
					}
				}
				else if (_month == 4)
				{
					DisplayAlert("UWAGA!", "Kwiecień ma tylko 30 dni!", "OK");
					return false;
				}
				else if (_month == 6)
				{
					DisplayAlert("UWAGA!", "Czerwiec ma tylko 30 dni!", "OK");
					return false;
				}
				else if (_month == 9)
				{
					DisplayAlert("UWAGA!", "Wrzesień ma tylko 30 dni!", "OK");
					return false;
				}
				else if (_month == 11)
				{
					DisplayAlert("UWAGA!", "Listopad ma tylko 30 dni!", "OK");
					return false;
				}
			}
			return true;
		}
		protected override bool OnBackgroundClicked()
		{
			return false;
		}
	}
}