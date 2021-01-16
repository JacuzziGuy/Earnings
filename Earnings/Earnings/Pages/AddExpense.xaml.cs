using Earnings.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Earnings.Pages
{
	public partial class AddExpense : ContentPage
	{
		int _day = DateTime.Now.Day, _month = DateTime.Now.Month, _year = DateTime.Now.Year;
		bool monthChosen = false, yearChosen = false;
		SQLiteConnection db = DBModel.DBPath();
		ObservableCollection<Expenses> _expenses = new ObservableCollection<Expenses>();
		public AddExpense(ObservableCollection<Expenses> expenses)
		{
			InitializeComponent();
			_expenses = expenses;
		}
		private void AddClicked(object sender, System.EventArgs e)
		{
			try
			{
				Expenses expense = new Expenses() { Cash = int.Parse(price.Text), Day = _day, Month = _month, Year = _year, Name = reason.Text };
				Total.ex += expense.Cash;
				db.Insert(expense);
				_expenses.Add(expense);
				Navigation.PopModalAsync();
			}
			catch 
			{
				DisplayAlert("Uwaga", "Proszę uzupełnić cenę", "OK");
			}
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
	}
}