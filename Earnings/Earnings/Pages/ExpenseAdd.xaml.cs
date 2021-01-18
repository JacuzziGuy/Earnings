using Earnings.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace Earnings.Pages
{
	public partial class ExpenseAdd : PopupPage
	{
		int _day = DateTime.Now.Day, _month = DateTime.Now.Month, _year = DateTime.Now.Year;
		SQLiteConnection db = DBModel.DBPath();
		ObservableCollection<ExpensesModel> _expenses = new ObservableCollection<ExpensesModel>();
		public ExpenseAdd(ObservableCollection<ExpensesModel> expenses)
		{
			InitializeComponent();
			_expenses = expenses;
			InitPage();
		}
		private void InitPage()
		{
			day.SelectedItem = _day.ToString();
			month.SelectedItem = _month.ToString();
			year.SelectedItem = _year.ToString();
		}
		private void AddClicked(object sender, System.EventArgs e)
		{
			try
			{
				ExpensesModel expense = new ExpensesModel() { Cash = int.Parse(price.Text), Day = _day, Month = _month, Year = _year, Name = reason.Text };
				Total.ex += expense.Cash;
				db.Insert(expense);
				_expenses.Add(expense);
				PopupNavigation.Instance.PopAsync();
			}
			catch 
			{
				DisplayAlert("Uwaga", "Proszę uzupełnić cenę", "OK");
			}
		}
		private void CancelClicked(object sender, System.EventArgs e)
		{
			PopupNavigation.Instance.PopAsync();
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
		protected override bool OnBackgroundClicked()
		{
			return false;
		}
	}
}