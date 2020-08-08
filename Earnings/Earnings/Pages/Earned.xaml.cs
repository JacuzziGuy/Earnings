using System;
using Xamarin.Forms;
using Earnings.Models;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;

namespace Earnings.Pages
{
	public partial class Earned : ContentPage
	{
		ObservableCollection<Earns> earns = new ObservableCollection<Earns>();
		int prevCount = 0;
		Earns selectedEarn = new Earns();
		private SQLiteConnection _conn;
		public Earned()
		{
			InitializeComponent();
			InitDB();
			InitList();
		}
		private void InitDB()
		{
			_conn = DependencyService.Get<ISQLite>().GetConnection();
			if (earns != null || earns.Count != 0)
			{
				earns.Clear();
				_conn.CreateTable<Earns>();
			}
			earns = new ObservableCollection<Earns>(_conn.Query<Earns>("select * from Earns"));
		}
		private void InitList()
		{
			earningsList.ItemsSource = earns;
			earningsList.ItemTapped += ShowHidden;
			Total.e = 0;
			for (int i = 0; i < earns.Count; i++)
			{
				Total.e += earns[i].Cash;
			}
		}
		private void ShowHidden(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as Earns;
			int index = e.ItemIndex;
			selectedEarn = item;
			ChangeVisibility(item, index);
		}
		private void ChangeVisibility(Earns item, int index)
		{
			for (int i = 0; i < earns.Count; i++)
			{
				if (earns[i] != item && earns[i].IsVisible == true)
				{
					earns[i].IsVisible = false;
					UpdateItems(earns[i], index);
				}
			}
			item.IsVisible = !item.IsVisible;
			UpdateItems(item, index);
		}
		private void UpdateItems(Earns item, int index)
		{
			earns.Remove(item);
			earns.Insert(index, item);
		}
		private void AddClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new NavigationPage(new EarningAdd(earns, false)));
		}
		private void AddFClicked(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new NavigationPage(new EarningAdd(earns, true)));
		}
		private void RemoveClicked(object sender, EventArgs e)
		{
			Total.e -= selectedEarn.Cash;
			_conn.Delete(selectedEarn);
			earns.Remove(selectedEarn);
		}
		protected override void OnAppearing()
		{
			if(prevCount != earns.Count)
			{
				prevCount = earns.Count;
				earns = SortItems(earns);
				Total.e = 0;
				for (int i = 0; i < earns.Count; i++)
				{
					Total.e += earns[i].Cash;
				}
				base.OnAppearing();
			}
		}
		private ObservableCollection<Earns> SortItems(ObservableCollection<Earns> orderList)
		{
			ObservableCollection<Earns> temp = new ObservableCollection<Earns>(orderList.OrderBy(i=>i.Day).OrderBy(i => i.Month).OrderBy(i=>i.Year).Reverse());
			orderList.Clear();
			foreach(Earns e in temp)
				orderList.Add(e);
			return orderList;
        }
	}
}