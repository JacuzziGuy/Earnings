using Earnings.Models;
using System;
using System.Linq;
using SQLite;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Earnings.Pages
{
	public partial class Expenses : ContentPage
	{
		ObservableCollection<ExpensesModel> expenses = new ObservableCollection<ExpensesModel>();
		int prevCount = 0;
		ExpensesModel selectedItem = new ExpensesModel();
		SQLiteConnection db = DBModel.DBPath();
		public Expenses()
		{
			InitializeComponent();
			InitDB();
			InitList();
		}
		private void InitDB()
		{
			try
			{
				expenses = new ObservableCollection<ExpensesModel>(db.Query<ExpensesModel>("select * from ExpensesModel"));
			}
			catch
			{
				db.CreateTable<ExpensesModel>();
			}
		}
		private void InitList()
		{
			expensesList.ItemsSource = expenses;
			expensesList.ItemTapped += ExpensesList_ItemTapped;
			Total.ex = expenses.Sum(x=>x.Cash);
		}

		private void ExpensesList_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as ExpensesModel;
			int index = e.ItemIndex;
			selectedItem = item;
			ChangeVisibility(item, index);
		}
		private void ChangeVisibility(ExpensesModel item, int index)
		{
			for (int i = 0; i < expenses.Count; i++)
			{
				if (expenses[i] != item && expenses[i].IsVisible == true)
				{
					expenses[i].IsVisible = false;
					UpdateItems(expenses[i], index);
				}
			}
			item.IsVisible = !item.IsVisible;
			UpdateItems(item, index);
		}
		private void UpdateItems(ExpensesModel item, int index)
		{
			expenses.Remove(item);
			expenses.Insert(index, item);
		}
		private void AddClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new NavigationPage(new ExpenseAdd(expenses)));
		}

		private void RemoveClicked(object sender, EventArgs e)
		{
			Total.ex -= selectedItem.Cash;
			db.Delete(selectedItem);
			expenses.Remove(selectedItem);
		}
		protected override void OnAppearing()
		{
			if (prevCount != expenses.Count)
			{
				prevCount = expenses.Count;
				expenses = SortItems(expenses);
				base.OnAppearing();
			}
		}
		private ObservableCollection<ExpensesModel> SortItems(ObservableCollection<ExpensesModel> orderList)
		{
			ObservableCollection<ExpensesModel> temp = new ObservableCollection<ExpensesModel>(orderList.OrderBy(i => i.Day).OrderBy(i => i.Month).OrderBy(i => i.Year).Reverse());
			orderList.Clear();
			foreach (ExpensesModel e in temp)
				orderList.Add(e);
			return orderList;
		}
	}
}