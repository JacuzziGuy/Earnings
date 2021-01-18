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
		ObservableCollection<EarningsModel> earns = new ObservableCollection<EarningsModel>();
		int prevCount = 0;
		EarningsModel selectedEarn = new EarningsModel();
		SQLiteConnection db = DBModel.DBPath();
		public Earned()
		{
			InitializeComponent();
			InitDB();
			InitList();
		}
		private void InitDB()
		{
			try
			{
				earns = new ObservableCollection<EarningsModel>(db.Query<EarningsModel>("select * from EarningsModel"));
			}
			catch
			{
				db.CreateTable<EarningsModel>();
			}
		}
		private void InitList()
		{
			earningsList.ItemsSource = earns;
			earningsList.ItemTapped += ShowHidden;
			Total.e = earns.Sum(x=>x.Cash);
		}

		private void ShowHidden(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as EarningsModel;
			int index = e.ItemIndex;
			selectedEarn = item;
			ChangeVisibility(item, index);
		}
		private void ChangeVisibility(EarningsModel item, int index)
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
		private void UpdateItems(EarningsModel item, int index)
		{
			earns.Remove(item);
			earns.Insert(index, item);
		}
		private void AddClicked(object sender, EventArgs e)
		{
			PopupNavigation.Instance.PushAsync(new EarningAdd(earns));
		}

		private void RemoveClicked(object sender, EventArgs e)
		{
			Total.e -= selectedEarn.Cash;
			db.Delete(selectedEarn);
			earns.Remove(selectedEarn);
		}
		protected override void OnAppearing()
		{
			if(prevCount != earns.Count)
			{
				prevCount = earns.Count;
				earns = SortItems(earns);
				base.OnAppearing();
			}
		}
		private ObservableCollection<EarningsModel> SortItems(ObservableCollection<EarningsModel> orderList)
		{
			ObservableCollection<EarningsModel> temp = new ObservableCollection<EarningsModel>(orderList.OrderBy(i=>i.Day).OrderBy(i => i.Month).OrderBy(i=>i.Year).Reverse());
			orderList.Clear();
			foreach(EarningsModel e in temp)
				orderList.Add(e);
			return orderList;
        }
	}
}