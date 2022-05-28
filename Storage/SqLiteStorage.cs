using Microsoft.Data.Sqlite;

namespace MList.Storage
{
    public class SqLiteStorage
    {
        private SqliteConnection _connection;

        SqLiteStorage(string connectionString)
        {
            this._connection = new SqliteConnection(connectionString);
            this._connection.Open();
        }

        public string AddAddress(string address)
        {
            string sqlExpression = "INSERT INTO addresses (address) VALUES (@address)";
        }
    }
    
    
}