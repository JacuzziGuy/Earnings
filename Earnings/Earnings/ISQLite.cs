using SQLite;

namespace Earnings
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();
	}
}
