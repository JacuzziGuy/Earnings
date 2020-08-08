using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms.Xaml;

namespace Earnings.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPickerValueEarnings
	{
		public AddPickerValueEarnings()
		{
			InitializeComponent();
		}
		private void SaveTimeClicked(object sender, EventArgs e)
		{
			Save(false);
		}
		private void SavePaidClicked(object sender, EventArgs e)
		{
			Save(true);
		}
		private void Save(bool _paid)
		{
			if (_paid)
			{
				string[] tempList = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/PaidValues.txt");
				string[] paidList = new string[tempList.Length + 1];
				for (int i = 0; i < tempList.Length; i++)
				{
					paidList[i] = tempList[i];
				}
				if(!paidList.Contains(entry.Text + "zł") && (entry.Text != "" && entry.Text != string.Empty && entry.Text != null))
				{
					paidList[tempList.Length] = entry.Text + "zł";
					paidList = paidList.OrderBy(x => float.Parse(x.Replace("zł", ""))).ToArray();
					File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/PaidValues.txt", paidList);
					EarningAdd.changed = true;
				}
			}
			else
			{
				string[] tempList = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/TimeValues.txt");
				string[] timeList = new string[tempList.Length + 1];
				for (int i = 0; i < tempList.Length; i++)
				{
					timeList[i] = tempList[i];
				}
				if (!timeList.Contains(entry.Text + "h") && (entry.Text != "" && entry.Text != string.Empty && entry.Text != null))
				{
					timeList[tempList.Length] = entry.Text + "h";
					timeList = timeList.OrderBy(x => float.Parse(x.Replace("h", ""))).ToArray();
					File.WriteAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/EarningData/TimeValues.txt", timeList);
					EarningAdd.changed = true;
				}
			}
			PopupNavigation.PopAsync();
		}
	}
}