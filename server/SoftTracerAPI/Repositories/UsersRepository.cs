using ExtensionMethods;
using MySql.Data.MySqlClient;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Models;
using System;
using System.Data;
using System.Text;

namespace SoftTracerAPI.Repositories
{
    public class UsersRepository

    {
        private readonly MySqlConnection _connection;

        public UsersRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region UserExists

        public bool UserExists(string userId)
        {
            MySqlCommand command = new MySqlCommand($"SELECT COUNT(0) FROM users WHERE username={ Extensions.SqlString(userId)}", _connection);
            return int.Parse(command.ExecuteScalar().ToString()) > 0;
        }

        #endregion UserExists

        #region EmailExists

        public bool EmailExists(string email)
        {
            MySqlCommand command = new MySqlCommand($"SELECT COUNT(0) FROM users WHERE email={ Extensions.SqlString(email)}", _connection);
            return int.Parse(command.ExecuteScalar().ToString()) > 0;
        }

        #endregion EmailExists

        #region FindUsernameByToken

        public string FindUsernameByToken(Guid token)
        {
            MySqlCommand command = new MySqlCommand($"SELECT username FROM users WHERE token=@token", _connection);
            command.Parameters.AddWithValue("@token", token).DbType = DbType.Guid;
            return command.ExecuteScalar().ToString();
        }

        #endregion FindUsernameByToken

        #region Authentication

        public Authentication FindAuthentication(FindAuthenticationCommand model)
        {
            Authentication result = null;
            MySqlCommand command = new MySqlCommand($"SELECT username,displayName,email,token FROM users WHERE username=@username AND password=@password", _connection);
            command.Parameters.AddWithValue("@username", model.UserId);
            command.Parameters.AddWithValue("@password", Extensions.EncryptString(model.Password));

            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = PopulateAuthentition(reader);
                }
            }

            return result;
        }

        private static Authentication PopulateAuthentition(IDataReader reader)
        {
            return new Authentication
            {
                DisplayName = reader["displayName"].ToString(),
                Email = reader["email"].ToString(),
                Token = new Guid(reader["token"].ToString()),
                UserId = reader["username"].ToString()
            };
        }

        #endregion Authentication

        #region CreateUser

        public void Create(CreateUserCommand model)
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
            sql.AppendLine("(username,displayname,password,email,token)").AppendLine();
            sql.AppendLine("VALUES").AppendLine();
            sql.AppendLine("(@username,@displayname,@password,@email,UUID())");
            return sql.ToString();
        }

        #endregion CreateUser

    }
}