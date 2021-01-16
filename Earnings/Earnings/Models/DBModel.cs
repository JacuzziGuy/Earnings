using System;
using SQLite;

namespace Earnings.Models
{
	public class DBModel
	{
		public static SQLiteConnection DBPath()
		{
			return new SQLiteConnection(Environment.GetFolderPath(Environment.SpecialFolder.Personal)+"/EarningsDataBase.sqlite");
		}
	}
}
