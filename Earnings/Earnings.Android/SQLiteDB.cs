using SQLite;
using Xamarin.Forms;
using Earnings.Droid;
using System.IO;

[assembly: Dependency(typeof(SQLiteDB))]
namespace Earnings.Droid
{
	public class SQLiteDB : ISQLite
	{
		public SQLiteConnection GetConnection()
		{
			var dbName = "EarningsDB.sqlite";
			var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "/EarningData";
			if (Directory.Exists(dbPath))
			{
				var conn = new SQLiteConnection($"{dbPath}/{dbName}");
				return conn;
			}
			else
			{
				Directory.CreateDirectory(dbPath);
				var conn = new SQLiteConnection($"{dbPath}/{dbName}");
				return conn;
			}
		}
    }
}