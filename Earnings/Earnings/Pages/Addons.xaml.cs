using Earnings.Models;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Rg.Plugins.Popup.Services;

namespace Earnings.Pages
{
	public partial class Addons : ContentPage
	{
		ObservableCollection<AddonsModel> addons = new ObservableCollection<AddonsModel>();
		int prevCount = 0;
		AddonsModel selectedItem = new AddonsModel();
		SQLiteConnection db = DBModel.DBPath();
		public Addons()
		{
			InitializeComponent();
			InitDB();
			InitList();
		}
		private void InitDB()
		{
			try
			{
				addons = new ObservableCollection<AddonsModel>(db.Query<AddonsModel>("select * from AddonsModel"));
			}
			catch
			{
				db.CreateTable<AddonsModel>();
			}
		}
		private void InitList()
		{
			addonsList.ItemsSource = addons;
			addonsList.ItemTapped += AddonsModelList_ItemTapped;
			Total.a = addons.Sum(x=>x.Cash);
		}

		private void AddonsModelList_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var item = e.Item as AddonsModel;
			int index = e.ItemIndex;
			selectedItem = item;
			ChangeVisibility(item, index);
		}
		private void ChangeVisibility(AddonsModel item, int index)
		{
			for (int i = 0; i < addons.Count; i++)
			{
				if (addons[i] != item && addons[i].IsVisible == true)
				{
					addons[i].IsVisible = false;
					UpdateItems(addons[i], index);
				}
			}
			item.IsVisible = !item.IsVisible;
			UpdateItems(item, index);
		}
		private void UpdateItems(AddonsModel item, int index)
		{
			addons.Remove(item);
			addons.Insert(index, item);
		}
		private void AddClicked(object sender, EventArgs e)
		{
			PopupNavigation.Instance.PushAsync(new AddonsAdd(addons));
		}

		private void RemoveClicked(object sender, EventArgs e)
		{
			Total.a -= selectedItem.Cash;
			db.Delete(selectedItem);
			addons.Remove(selectedItem);
		}

		protected override void OnAppearing()
		{
			if (prevCount != addons.Count)
			{
				prevCount = addons.Count;
				addons = SortItems(addons);
				base.OnAppearing();
			}
		}
		private ObservableCollection<AddonsModel> SortItems(ObservableCollection<AddonsModel> orderList)
		{
			ObservableCollection<AddonsModel> temp = new ObservableCollection<AddonsModel>(orderList.OrderBy(i => i.Day).OrderBy(i => i.Month).OrderBy(i => i.Year).Reverse());
			orderList.Clear();
			foreach (AddonsModel e in temp)
				orderList.Add(e);
			return orderList;
		}
	}
}