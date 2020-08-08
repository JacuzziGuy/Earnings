using SQLite;

namespace Earnings.Models
{
	public class PaidModel
	{
		[AutoIncrement, PrimaryKey]
		public int Id { get; set; }
		public string Paid { get; set; }
	}
}
