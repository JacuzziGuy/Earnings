using Earnings.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Earnings.Pages
{
	public partial class AddonsAdd : ContentPage
	{
		int _day = 1, _month = 1, _year = 2020;
		bool monthChosen = false, yearChosen = false, created = false;
		SQLiteConnection _conn;
		ObservableCollection<AddonsModel> _addons = new ObservableCollection<AddonsModel>();
		public AddonsAdd(ObservableCollection<AddonsModel> addons)
		{
			InitializeComponent();
			SetDateStart();
			SetList(addons);
		}
		public AddonsAdd(int Day, int Month, int Year, ObservableCollection<AddonsModel> addons)
		{
			InitializeComponent();
			GetDate(Day, Month, Year);
			SetList(addons);
		}
		private void AddClicked(object sender, System.EventArgs e)
		{
			AddItem();
			if (created == false)
				return;
			else
				Navigation.PopModalAsync();
		}
		private void AddPlusClicked(object sender, System.EventArgs e)
		{
			AddItem();
			if (created == false)
				return;
			else
			{
				Navigation.PopModalAsync();
				Navigation.PushModalAsync(new AddonsAdd(_day, _month, _year, _addons));
			}
		}
		private void SetDateStart()
		{
			day.SelectedItem = "1";
			month.SelectedItem = "1";
			year.SelectedItem = "2020";
			monthChosen = false;
			yearChosen = false;
		}
		private void SetList(ObservableCollection<AddonsModel> addons)
		{
			_addons = addons;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
		}
		private void GetDate(int Day, int Month, int Year)
		{
			Day += 1;
			if (Day > 31)
			{
				Day = 1;
				Month += 1;
			}
			if (Month > 12)
			{
				Month = 1;
				Year += 1;
			}
			_day = Day;
			_month = Month;
			_year = Year;
			day.SelectedItem = _day.ToString();
			month.SelectedItem = _month.ToString();
			year.SelectedItem = _year.ToString();
			monthChosen = true;
			yearChosen = true;
		}
		private void CancelClicked(object sender, System.EventArgs e)
		{
			Navigation.PopModalAsync();
		}
		private void day_SelectedIndexChanged(object sender, EventArgs e)
		{
			_day = int.Parse(day.SelectedItem.ToString());
			if (monthChosen == false)
				month.Focus();
		}
		private void month_SelectedIndexChanged(object sender, EventArgs e)
		{
			_month = int.Parse(month.SelectedItem.ToString());
			monthChosen = true;
			if (yearChosen == false)
				year.Focus();
		}
		private void year_SelectedIndexChanged(object sender, EventArgs e)
		{
			_year = int.Parse(year.SelectedItem.ToString());
			yearChosen = true;
		}
		private void AddItem()
		{
			if (price.Text == null)
			{
				DisplayAlert("UWAGA!", "Uzupełnij pole ilości!", "OK");
				return;
			}
			if (reason.Text == null)
			{
				DisplayAlert("UWAGA!", "Napisz za co jest ten dodatek!", "OK");
				return;
			}
			AddonsModel addon = new AddonsModel() { Cash = int.Parse(price.Text), Day = _day, Month = _month, Year = _year, Name = reason.Text };
			if (_month == 2 && _day > 28 || _month == 4 && _day == 31 || _month == 6 && _day == 31 || _month == 9 && _day == 31 || _month == 11 && _day == 31)
			{
				if (_month == 2)
				{
					if (_year % 4 != 0)
					{
						DisplayAlert("UWAGA!", "Luty w roku " + _year + " ma tylko 28 dni!", "OK");
						return;
					}
					else if (_day > 29 && _year % 4 == 0)
					{
						DisplayAlert("UWAGA!", "Luty w roku " + _year + " ma tylko 29 dni!", "OK");
						return;
					}
				}
				else if (_month == 4)
				{
					DisplayAlert("UWAGA!", "Kwiecień ma tylko 30 dni!", "OK");
					return;
				}
				else if (_month == 6)
				{
					DisplayAlert("UWAGA!", "Czerwiec ma tylko 30 dni!", "OK");
					return;
				}
				else if (_month == 9)
				{
					DisplayAlert("UWAGA!", "Wrzesień ma tylko 30 dni!", "OK");
					return;
				}
				else if (_month == 11)
				{
					DisplayAlert("UWAGA!", "Listopad ma tylko 30 dni!", "OK");
					return;
				}
			}
			Total.a += addon.Cash;
			_conn.Insert(addon);
			_addons.Add(addon);
			created = true;
		}
	}
}