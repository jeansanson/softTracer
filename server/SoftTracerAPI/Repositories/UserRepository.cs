using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Transactions;

namespace SoftTracerAPI.Repositories
{
    public class UserRepository
    {
        private MySqlConnection _connection;

        public UserRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

    }
}