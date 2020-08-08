using SQLite;

namespace Earnings.Models
{
	public class TimeModel
	{
		[AutoIncrement, PrimaryKey]
		public int Id { get; set; }
		public string Time { get; set; }
	}
}
