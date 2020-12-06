using MySql.Data.MySqlClient;
using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Models.Projects.Requirements;
using SofTracerAPI.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ExtensionMethods;

namespace SoftTracerAPI.Repositories
{
    public class RequirementsRepository

    {
        private readonly MySqlConnection _connection;

        public RequirementsRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region Create

        public void Create(int projectId, List<CreateRequirementsCommand> command)
        {
            List<Requirement> requirements = new RequirementsService().MapCommand(command);
            Delete(projectId);
            foreach (Requirement requirement in requirements)
            {
                Create(projectId, requirement);
                if (requirement.Children != null)
                {
                    foreach (Requirement childRequirement in requirement.Children)
                    {
                        Create(projectId, childRequirement);
                    }
                }
            }
        }

        private void Create(int projectId, Requirement requirement)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateQuery();
            PopulateCreateCommand(projectId, requirement, command);
            command.ExecuteNonQuery();
        }

        private static void PopulateCreateCommand(int projectId, Requirement requirement, MySqlCommand command)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = requirement.Id;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = requirement.Name;
            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = requirement.Description;
            command.Parameters.Add("@completed", MySqlDbType.Bit).Value = requirement.Completed;
            command.Parameters.Add("@parentId", MySqlDbType.Int32).Value = requirement.ParentId;
        }

        private string GetCreateQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO requirements (");
            sql.AppendLine("projectId");
            sql.AppendLine(",requirementId");
            sql.AppendLine(",name");
            sql.AppendLine(",description");
            sql.AppendLine(",completed");
            sql.AppendLine(",parentId)");
            sql.AppendLine("VALUES (");
            sql.AppendLine("@projectId");
            sql.AppendLine(",@requirementId");
            sql.AppendLine(",@name");
            sql.AppendLine(",@description");
            sql.AppendLine(",@completed");
            sql.AppendLine(",@parentId)");
            return sql.ToString();
        }

        #endregion Create

        #region Delete

        public void Delete(int projectId, int requirementId)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM requirements WHERE projectId=@projectId AND requirementId=@requirementId OR parentId=@parentId";
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = requirementId;
            command.Parameters.Add("@parentId", MySqlDbType.Int32).Value = requirementId;
            command.ExecuteNonQuery();
        }

        public void Delete(int projectId)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM requirements WHERE projectId=@projectId";
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.ExecuteNonQuery();
        }

        #endregion Delete

        #region Update

        public void Update(int projectId, List<Requirement> requirements)
        {
            foreach (Requirement requirement in requirements)
            {
                Delete(projectId, requirement.Id);
                Create(projectId, requirement);
                if (requirement.Children == null) return;
                foreach (Requirement childRequirement in requirement.Children)
                {
                    Delete(projectId, childRequirement.Id);
                    Create(projectId, childRequirement);
                }
            }
        }

        #endregion Update

        #region Find

        public List<Requirement> Find(int projectId)
        {
            List<Requirement> everyRequirement = new List<Requirement>();
            Find(projectId, everyRequirement);
            return everyRequirement.Where(item => item.ParentId == 0).ToList();
        }

        public Requirement Find(int projectId, int requirementId)
        {
            List<Requirement> everyRequirement = new List<Requirement>();
            Find(projectId, everyRequirement);
            return everyRequirement.FirstOrDefault(item => item.Id == requirementId);
        }

        private void Find(int projectId, List<Requirement> everyRequirement)
        {
            MySqlCommand command = new MySqlCommand(GetFindRequirementsQuery(), _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    everyRequirement.Add(PopulateRequirement(reader));
                }
            }
            PopulateParents(everyRequirement);
        }

        private static void PopulateParents(List<Requirement> everyRequirement)
        {
            List<Requirement> childs = everyRequirement.Where(item => item.ParentId > 0).ToList();
            foreach (Requirement child in childs)
            {
                Requirement parent = everyRequirement.FirstOrDefault(item => item.Id == child.ParentId);
                if (parent != null) parent.Children.Add(child);
            }
        }

        private static Requirement PopulateRequirement(IDataReader reader)
        {
            return new Requirement
            {
                Id = int.Parse(reader["requirementId"].ToString()),
                ParentId = int.Parse(reader["parentId"].ToString()),
                Name = reader["name"].ToString(),
                Description = reader["description"].ToString(),
                Completed = Extensions.ToBool(reader["completed"].ToString()),
                Children = new List<Requirement>(),
            };
        }

        private string GetFindRequirementsQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("requirementId");
            sql.AppendLine(",parentId");
            sql.AppendLine(",name");
            sql.AppendLine(",description");
            sql.AppendLine(",completed");
            sql.AppendLine("FROM requirements");
            sql.AppendLine("WHERE projectId=@projectId");
            return sql.ToString();
        }

        #endregion Find
    }
}