using Earnings.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Earnings.Pages
{
	public partial class AddExpense : ContentPage
	{
		int _day = int.Parse(System.DateTime.Now.ToString("dd"))
			, _month = int.Parse(System.DateTime.Now.ToString("MM"))
			, _year = int.Parse(System.DateTime.Now.ToString("yyyy"));
		bool monthChosen = false, yearChosen = false, fastSave, created = false;
		SQLiteConnection _conn;
		ObservableCollection<Expenses> _expenses = new ObservableCollection<Expenses>();
		public AddExpense(ObservableCollection<Expenses> expenses, bool fSave)
		{
			fastSave = fSave;
			InitializeComponent();
			SetSave();
			SetDate();
			_expenses = expenses;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
		}
		public AddExpense(int Day, int Month, int Year, ObservableCollection<Expenses> expenses, bool fSave)
		{
			fastSave = fSave;
			InitializeComponent();
			SetSave();
			GetDate(Day, Month, Year);
			_expenses = expenses;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
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
		private void SetSave()
		{
			if (fastSave)
				saveButton.Clicked += AddFClicked;
			else
				saveButton.Clicked += AddClicked;
		}
		private void AddClicked(object sender, System.EventArgs e)
		{
			AddItem();
			if (created)
				Navigation.PopModalAsync();
			else
				return;
		}
		private void AddFClicked(object sender, System.EventArgs e)
		{
			AddItem();
			if (created)
			{
				Navigation.PopModalAsync();
				Navigation.PushModalAsync(new NavigationPage(new AddExpense(_day, _month, _year, _expenses, true)));
			}
			else
				return;
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
				DisplayAlert("UWAGA!", "Napisz ile wydałeś!", "OK");
				return;
			}
			if (reason.Text == null)
			{
				DisplayAlert("UWAGA!", "Napisz na co wydałeś!", "OK");
				return;
			}
			Expenses expense = new Expenses() { Cash = int.Parse(price.Text), Day = _day, Month = _month, Year = _year, Name = reason.Text };
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
			Total.ex += expense.Cash;
			_conn.Insert(expense);
			_expenses.Add(expense);
			created = true;
		}
	}
}