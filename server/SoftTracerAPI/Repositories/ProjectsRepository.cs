using MySql.Data.MySqlClient;
using SofTracerAPI.Models.Projects;
using SoftTracerAPI.Commands.Projects;
using System;
using System.Collections.Generic;
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

        #region Create

        public Project Create(CreateProjectCommand model)
        {
            int projectId = FindNextId();
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateCommandText();
            PopulateCreateCommand(model, command, projectId);
            command.ExecuteNonQuery();
            Project project = Find(projectId);
            AddUser(project.Token, UserRole.Administrator);
            return project;
        }

        private void PopulateCreateCommand(CreateProjectCommand model, MySqlCommand command, int projectId)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@resume", MySqlDbType.VarChar).Value = model.Resume;
            command.Parameters.Add("@openingDate", MySqlDbType.DateTime).Value = DateTime.Now;
        }

        static private string GetCreateCommandText()
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
            MySqlCommand command = new MySqlCommand($"SELECT IFNULL(MAX(projectId) + 1,1) FROM projects", _connection);
            return int.Parse(command.ExecuteScalar().ToString());
        }

        #endregion CreateProject

        #region Find

        public Project Find(int id)
        {
            Project result = null;
            StringBuilder query = new StringBuilder(GetFindProjectsQuery());
            query.AppendLine("WHERE PRO.projectId=@projectId");
            query.AppendLine("AND US.username=@username");
            MySqlCommand command = new MySqlCommand(query.ToString(), _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = id;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = _username.Identity.Name;
            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = PopulateProject(reader);
                }
            }

            return result;
        }

        public Project Find(Guid token)
        {
            int id = FindId(token);
            return Find(id);
        }

        static private string GetFindProjectsQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT PRO.projectId");
            sql.AppendLine(",PRO.name");
            sql.AppendLine(",PRO.openingDate");
            sql.AppendLine(",PRO.token");
            sql.AppendLine(",PRO.resume FROM projects PRO");
            sql.AppendLine("JOIN projects_users US ON PRO.projectId=US.projectId");
            return sql.ToString();
        }

        private static Project PopulateProject(IDataReader reader)
        {
            return new Project
            {
                Name = reader["name"].ToString(),
                Resume = reader["resume"].ToString(),
                Token = new Guid(reader["token"].ToString()),
                OpeningDate = DateTime.Parse(reader["openingDate"].ToString()),
                Id = int.Parse(reader["projectId"].ToString())
            };
        }

        public List<Project> Find()
        {
            List<Project> result = new List<Project>();
            StringBuilder query = new StringBuilder(GetFindProjectsQuery());
            query.AppendLine("WHERE US.username=@username");
            MySqlCommand command = new MySqlCommand(query.ToString(), _connection);
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = _username.Identity.Name;

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(PopulateProject(reader));
                }
            }

            return result;
        }

        #endregion Find

        #region AddUser

        public void AddUser(Guid projectToken, UserRole role)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetAddUserCommandText();
            PopulateAddUserCommand(projectToken, role, _username.Identity.Name, command);
            command.ExecuteNonQuery();
        }

        public void DeleteUser(int projectId, string username)
        {
            UserRole role = FindUserRole(projectId, username);
            if (role == UserRole.Guest && username != _username.Identity.Name) { return; }
            MySqlCommand command = new MySqlCommand("DELETE FROM projects_users WHERE username=@username AND projectId=@projectId", _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            command.ExecuteNonQuery();
        }

        public void AddUser(Guid projectToken, string username, UserRole role)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetAddUserCommandText();
            PopulateAddUserCommand(projectToken, role, username, command);
            command.ExecuteNonQuery();
        }

        private UserRole FindUserRole(int projectId, string username)
        {
            MySqlCommand command = new MySqlCommand($"SELECT role FROM projects_users WHERE projectId=@projectId AND username=@username", _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@username", MySqlDbType.VarChar).Value = username;
            return (UserRole)int.Parse(command.ExecuteScalar().ToString());
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
            sql.AppendLine("INSERT INTO projects_users");
            sql.AppendLine("(projectId,username,role)");
            sql.AppendLine("VALUES").AppendLine();
            sql.AppendLine("(@projectId,@username,@role)");
            return sql.ToString();
        }

        #endregion AddUser

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