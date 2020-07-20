using Earnings.Models;
using System;
using System.Linq;
using SQLite;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Earnings.Pages
{
	public partial class Spent : ContentPage
	{
		ObservableCollection<Expenses> expenses = new ObservableCollection<Expenses>();
		int prevCount = 0;
		Expenses selectedItem = new Expenses();
		SQLiteConnection _conn;
		public Spent()
		{
			InitializeComponent();
			InitDB();
			InitList();
		}
		private void InitDB()
		{
			_conn = DependencyService.Get<ISQLite>().GetConnection();
			if(expenses == null || expenses.Count == 0)
			{
				expenses.Clear();
				_conn.CreateTable<Expenses>();
			}
			expenses = new ObservableCollection<Expenses>(_conn.Query<Expenses>("select * from Expenses"));
		}
		private void InitList()
		{
			expensesList.ItemsSource = expenses;
			expensesList.ItemTapped += ExpensesList_ItemTapped;
			Total.ex = 0;
			for (int i = 0; i < expenses.Count; i++)
			{
				Total.ex += expenses[i].Cash;
			}
		}

		private void ExpensesList_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as Expenses;
			int index = e.ItemIndex;
			selectedItem = item;
			ChangeVisibility(item, index);
		}
		private void ChangeVisibility(Expenses item, int index)
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
		private void UpdateItems(Expenses item, int index)
		{
			expenses.Remove(item);
			expenses.Insert(index, item);
		}
		private void AddClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new NavigationPage(new AddExpense(expenses)));
		}

		private void RemoveClicked(object sender, EventArgs e)
		{
			Total.ex -= selectedItem.Cash;
			_conn.Delete(selectedItem);
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
		private ObservableCollection<Expenses> SortItems(ObservableCollection<Expenses> orderList)
		{
			ObservableCollection<Expenses> temp = new ObservableCollection<Expenses>(orderList.OrderBy(i => i.Day).OrderBy(i => i.Month).OrderBy(i => i.Year).Reverse());
			orderList.Clear();
			foreach (Expenses e in temp)
				orderList.Add(e);
			return orderList;
		}
	}
}