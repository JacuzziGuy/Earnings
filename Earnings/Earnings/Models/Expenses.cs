using SQLite;
namespace Earnings.Models
{
	public class Expenses
	{
		[AutoIncrement, PrimaryKey]
		public int Id { get; set; }
		public bool IsVisible { get; set; }
		public int Cash { get; set; }
		public int Day { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Date { get { return Day + "." + Month + "." + Year; } }
		public string Text { get { return Date + " - " + Cash.ToString() + " zł"; } }
		public string Name { get; set; }
	}
}
