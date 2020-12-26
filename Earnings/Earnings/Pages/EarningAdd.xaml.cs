using SQLite;
using Earnings.Models;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Earnings.Pages
{
	public partial class EarningAdd : ContentPage
	{
		int _paid = 0, _time = 0, _day = 1, _month = 1, _year = 2020;
		bool paidChosen = false, timeChosen = false, dayChosen = false, monthChosen = false, yearChosen = false;
		ObservableCollection<Earns> _earns = new ObservableCollection<Earns>();
		private SQLiteConnection _conn;
		public EarningAdd(ObservableCollection<Earns> earns)
		{
			InitializeComponent();
			_earns = earns;
			_conn = DependencyService.Get<ISQLite>().GetConnection();
		}

		private void AddClicked(object sender, EventArgs e)
		{
			int cash = _paid * _time;
			Earns earn = new Earns { Cash = cash, IsVisible = false, Day = _day, Month = _month, Year = _year };
			Total.e += cash;
			_conn.Insert(earn);
			_earns.Add(earn);
			Navigation.PopModalAsync();
		}
		private void CancelClicked(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

		private void paid_SelectedIndexChanged(object sender, EventArgs e)
		{
			_paid = int.Parse(paid.SelectedItem.ToString().Replace("zł", ""));
			paidChosen = true;
			if (dayChosen == false)
				day.Focus();
		}

		private void time_SelectedIndexChanged(object sender, EventArgs e)
		{
			_time = int.Parse(time.SelectedItem.ToString().Replace("h", ""));
			timeChosen = true;
			if (paidChosen == false)
				paid.Focus();
		}

		private void day_SelectedIndexChanged(object sender, EventArgs e)
		{
			_day = int.Parse(day.SelectedItem.ToString());
			dayChosen = true;
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
	}
}