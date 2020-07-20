using SQLite;
using Xamarin.Forms;
using Earnings.Droid;

[assembly: Dependency(typeof(SQLiteDB))]
namespace Earnings.Droid
{
	public class SQLiteDB : ISQLite
	{
		public SQLiteConnection GetConnection()
		{
			var dbName = "EarningsDB.sqlite";
			var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
			var conn = new SQLiteConnection($"{dbPath}/{dbName}");
			return conn;
		}
    }
}