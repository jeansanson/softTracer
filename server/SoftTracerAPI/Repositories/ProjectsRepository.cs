using MySql.Data.MySqlClient;
using SofTracerAPI.Models.Projects;
using SoftTracerAPI.Commands.Projects;
using System;
using System.Data;
using System.Security.Principal;
using System.Text;

namespace SoftTracerAPI.Repositories
{
    public class ProjectsRepository

    {
        private readonly MySqlConnection _connection;
        private readonly IPrincipal _username;

        public ProjectsRepository(MySqlConnection connection, IPrincipal username)
        {
            _connection = connection;
            _username = username;
        }

        #region CreateProject

        public Project CreateProject(CreateProjectCommand model)
        {
            int projectId = FindNextId();
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateProjectCommandText();
            PopulateCreateProjectCommand(model, command, projectId);
            command.ExecuteNonQuery();
            Project project = FindProjectById(projectId);
            AddUser(project.Token, UserRole.Administrator);
            return project;
        }

        private void PopulateCreateProjectCommand(CreateProjectCommand model, MySqlCommand command, int projectId)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@resume", MySqlDbType.VarChar).Value = model.Resume;
            command.Parameters.Add("@openingDate", MySqlDbType.Date).Value = new DateTime();
        }

        static private string GetCreateProjectCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO projects").AppendLine();
            sql.AppendLine("(projectId,name,resume,openingDate,token)").AppendLine();
            sql.AppendLine("VALUES").AppendLine();
            sql.AppendLine("(@projectId,@name,@resume,@openingDate,UUID())");
            return sql.ToString();
        }

        private int FindNextId()
        {
            MySqlCommand command = new MySqlCommand($"SELECT MAX(projectId) + 1 FROM projects", _connection);
            return int.Parse(command.ExecuteScalar().ToString());
        }

        #endregion CreateProject

        #region FindProject

        public Project FindProjectById(int id)
        {
            Project result = null;
            MySqlCommand command = new MySqlCommand(GetFindProjectByIdQuery(), _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = id;

            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = PopulateProject(reader, id);
                }
            }

            return result;
        }

        static private string GetFindProjectByIdQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT name,resume,openingDate,token FROM projects");
            sql.AppendLine("WHERE projectId=@projectId");
            return sql.ToString();
        }

        private static Project PopulateProject(IDataReader reader, int id)
        {
            return new Project
            {
                Name = reader["name"].ToString(),
                Resume = reader["resume"].ToString(),
                Token = new Guid(reader["token"].ToString()),
                OpeningDate = DateTime.Parse(reader["openingDate"].ToString()),
                Id = id
            };
        }

        #endregion FindProject

        #region AddUser
        public void AddUser(Guid projectToken, UserRole role)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetAddUserCommandText();
            PopulateAddUserCommand(projectToken, role, _username.Identity.Name, command);
            command.ExecuteNonQuery();
        }

        public void AddUser(Guid projectToken, string username, UserRole role)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetAddUserCommandText();
            PopulateAddUserCommand(projectToken, role, username, command);
            command.ExecuteNonQuery();
        }

        private void PopulateAddUserCommand(Guid projectToken, UserRole role, string username, MySqlCommand command)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = FindId(projectToken);
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@role", MySqlDbType.Int32).Value = role;
        }

        static private string GetAddUserCommandText()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO projects_users").AppendLine();
            sql.AppendLine("(projectId,username,role)").AppendLine();
            sql.AppendLine("VALUES").AppendLine();
            sql.AppendLine("(@projectId,@username,@role");
            return sql.ToString();
        }

        #endregion

        public Guid FindToken(int id)
        {
            MySqlCommand command = new MySqlCommand($"SELECT token FROM projects WHERE projectId=@projectId", _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = id;
            return new Guid(command.ExecuteScalar().ToString());
        }

        public int FindId(Guid projectToken)
        {
            MySqlCommand command = new MySqlCommand($"SELECT projectId FROM projects WHERE token=@token", _connection);
            command.Parameters.Add("@token", MySqlDbType.Guid).Value = projectToken;
            return int.Parse(command.ExecuteScalar().ToString());
        }
    }
}