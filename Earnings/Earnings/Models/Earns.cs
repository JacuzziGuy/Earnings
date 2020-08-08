using SQLite;

namespace Earnings.Models
{
	public class Earns
	{
		[AutoIncrement, PrimaryKey]
		public int Id { get; set; }
		public bool IsVisible { get; set; }
		public float Cash { get; set; }
		public int Day { get; set; }
		public int Month { get; set; }
		public int Year { get; set; }
		public string Date { get { return (Day<10?"0"+Day.ToString():Day.ToString()) + "." + (Month<10?"0"+Month.ToString():Month.ToString()) + "." + Year.ToString(); } }
		public string Text { get { return Date+" - "+Cash.ToString() + " zł"; } }
	}
}
