using ExtensionMethods;
using MySql.Data.MySqlClient;
using SoftTracerAPI.Commands.Users;
using System.Text;

namespace SoftTracerAPI.Repositories
{
    public class UsersRepository

    {
        private MySqlConnection _connection;

        public UsersRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region UserExists
        public bool UserExists(string userId)
        {
            MySqlCommand command = new MySqlCommand(GetUserExistsQuery(userId), _connection);
            return int.Parse(command.ExecuteScalar().ToString()) > 0;
        }

        private static string GetUserExistsQuery(string userId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT COUNT(0) FROM users WHERE username={ Extensions.SqlString(userId)}");
            return sql.ToString();
        }

        #endregion

        #region EmailExists
        public bool EmailExists(string email)
        {
            MySqlCommand command = new MySqlCommand(GetEmailExistsQuery(email), _connection);
            return int.Parse(command.ExecuteScalar().ToString()) > 0;
        }

        private static string GetEmailExistsQuery(string email)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine($"SELECT COUNT(0) FROM users WHERE email={ Extensions.SqlString(email)}");
            return sql.ToString();
        }

        #endregion


        #region CreateUser

        public void CreateUser(CreateUserCommand model)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateUserCommandText();
            CreateUserPopulateParameters(model, command);
            command.ExecuteNonQuery();
        }

        private static void CreateUserPopulateParameters(CreateUserCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = model.UserId;
            command.Parameters.Add("@displayname", MySqlDbType.VarChar).Value = model.DisplayName;
            command.Parameters.Add("@password", MySqlDbType.VarChar).Value = Extensions.EncryptString(model.Password);
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = model.Email;
        }

        static private string GetCreateUserCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO users").AppendLine();
            sql.AppendLine("(username,displayname,password,email)").AppendLine();
            sql.AppendLine("VALUES").AppendLine();
            sql.AppendLine("(@username,@displayname,@password,@email)");
            return sql.ToString();
        }

        #endregion CreateUser
    }
}