using SQLite;
using Earnings.Models;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using System.Collections.Generic;

namespace Earnings.Pages
{
	public partial class EarningAdd : ContentPage
	{
		float _paid, _time;
		public static bool changed = false;
		int _day = int.Parse(System.DateTime.Now.ToString("dd"))
			, _month = int.Parse(System.DateTime.Now.ToString("MM"))
			, _year = int.Parse(System.DateTime.Now.ToString("yyyy"));
		bool paidChosen = false, timeChosen = false, monthChosen = false, yearChosen = false, created = false, fastSave;
		string timeValuesPath, paidValuesPath;
		ObservableCollection<Earns> _earns = new ObservableCollection<Earns>();
		private SQLiteConnection _conn;
		public EarningAdd(int Day, int Month, int Year, ObservableCollection<Earns> earns, bool fSave)
		{
			fastSave = fSave;
			InitializeComponent();
			SetSave();
			SetPickerValues();
			GetDate(Day, Month, Year);
			_earns = earns;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
		}
		public EarningAdd(ObservableCollection<Earns> earns, bool fSave)
		{
			fastSave = fSave;
			InitializeComponent();
			SetDate();
			SetSave();
			SetPickerValues();
			_earns = earns;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
		}
		private void SetPickerValues()
		{
			string[] items = File.ReadAllLines(paidValuesPath);
			paid.Items.Clear();
			time.Items.Clear();
			foreach(string item in items)
			{
				paid.Items.Add(item);
			}
			items = File.ReadAllLines(timeValuesPath);
			foreach(string item in items)
			{
				time.Items.Add(item);
			}
		}
		private void SetSave()
		{
			timeValuesPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/TimeValues.txt";
			paidValuesPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/PaidValues.txt";
			if (fastSave)
				saveButton.Clicked += AddFClicked;
			else
				saveButton.Clicked += AddClicked;
		}
		private void SetDate()
		{
			day.SelectedIndex = _day - 1;
			month.SelectedIndex = _month - 1;
			year.SelectedItem = _year.ToString();
			monthChosen = false;
			yearChosen = false;
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
			day.SelectedIndex = _day - 1;
			month.SelectedIndex = _month - 1;
			year.SelectedItem = _year.ToString();
			monthChosen = true;
			yearChosen = true;
		}
		private void AddClicked(object sender, EventArgs e)
		{
			AddItem();
			if (created == false)
				return;
			else
				Navigation.PopModalAsync();
		}
		private async void NewValueClicked(object sender, EventArgs e)
		{
			await PopupNavigation.PushAsync(new AddPickerValueEarnings());
			while (!changed)
			{
				await Task.Delay(300);
			}
			changed = false;
			SetPickerValues();
		}
		private async void DeleteValueClicked(object sender, EventArgs e)
		{
			string action = await DisplayActionSheet("Wybierz co chcesz usunąć", "", "ANULUJ", "Godzina", "Zarobek");
			switch (action)
			{
				case "Godzina":
					DeleteTime();
					break;
				case "Zarobek":
					DeletePaid();
					break;
			}
		}
		private async void DeleteTime()
		{
			string item = await DisplayActionSheet("Wybierz godzinę do usunięcia", "", "ANULUJ", File.ReadAllLines(timeValuesPath));
			time.Items.Remove(item);
			File.WriteAllLines(timeValuesPath, time.Items.ToArray());
		}
		private async void DeletePaid()
		{
			string item = await DisplayActionSheet("Wybierz zarobek do usunięcia", "", "ANULUJ", File.ReadAllLines(paidValuesPath));
			paid.Items.Remove(item);
			File.WriteAllLines(paidValuesPath, paid.Items.ToArray());
		}
		private void AddFClicked(object sender, EventArgs e)
		{
			AddItem();
			if (created == false)
				return;
			else
			{
				Navigation.PopModalAsync();
				Navigation.PushModalAsync(new NavigationPage(new EarningAdd(_day, _month, _year, _earns, true)));
			}
		}
		private void CancelClicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}
		private void paid_SelectedIndexChanged(object sender, EventArgs e)
		{
			_paid = float.Parse(paid.SelectedItem.ToString().Replace("zł", ""));
			paidChosen = true;
		}
		private void time_SelectedIndexChanged(object sender, EventArgs e)
		{
			_time = float.Parse(time.SelectedItem.ToString().Replace("h", ""));
			timeChosen = true;
			if (paidChosen == false)
				paid.Focus();
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
			if (timeChosen == false)
				time.Focus();
		}
		private void AddItem()
		{
			if (paid.SelectedItem == null)
			{
				DisplayAlert("UWAGA!", "Wybierz zarobek za godzinę!", "OK");
				return;
			}
			if (time.SelectedItem == null)
			{
				DisplayAlert("UWAGA!", "Wybierz ilość godzin!", "OK");
				return;
			}
			float cash = _paid * _time;
			Earns earn = new Earns { Cash = cash, IsVisible = false, Day = _day, Month = _month, Year = _year };
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
			Total.e += cash;
			_conn.Insert(earn);
			_earns.Add(earn);
			created = true;
		}
	}
}