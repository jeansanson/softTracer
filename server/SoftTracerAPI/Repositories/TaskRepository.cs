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
    public class TaskRepository

    {
        private readonly MySqlConnection _connection;

        public TaskRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region CreateRequirements

        public void CreateRequirements(int projectId, List<CreateRequirementsCommand> command)
        {
            List<Requirement> requirements = new RequirementsService().MapCommand(command);
            DeleteRequirement(projectId);
            foreach (Requirement requirement in requirements)
            {
                CreteRequirement(projectId, requirement);
                if (requirement.Children != null)
                {
                    foreach (Requirement childRequirement in requirement.Children)
                    {
                        CreteRequirement(projectId, childRequirement);
                    }
                }
            }
        }

        private void CreteRequirement(int projectId, Requirement requirement)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateRequirementQuery();
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

        private string GetCreateRequirementQuery()
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

        #endregion CreateRequirements

        #region DeleteRequirements

        public void DeleteRequirement(int projectId, int requirementId)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM requirements WHERE projectId=@projectId AND requirementId=@requirementId OR parentId=@parentId";
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = requirementId;
            command.Parameters.Add("@parentId", MySqlDbType.Int32).Value = requirementId;
            command.ExecuteNonQuery();
        }

        public void DeleteRequirement(int projectId)
        {
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM requirements WHERE projectId=@projectId";
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.ExecuteNonQuery();
        }

        #endregion DeleteRequirements

        #region UpdateRequirements

        public void UpdateRequirements(int projectId, List<Requirement> requirements)
        {
            foreach (Requirement requirement in requirements)
            {
                DeleteRequirement(projectId, requirement.Id);
                CreteRequirement(projectId, requirement);
                if (requirement.Children == null) return;
                foreach (Requirement childRequirement in requirement.Children)
                {
                    DeleteRequirement(projectId, childRequirement.Id);
                    CreteRequirement(projectId, childRequirement);
                }
            }
        }

        #endregion UpdateRequirements

        #region FindRequirements

        public List<Requirement> FindRequirements(int projectId)
        {
            List<Requirement> everyRequirement = new List<Requirement>();
            FindAllRequirements(projectId, everyRequirement);
            return everyRequirement.Where(item => item.ParentId == 0).ToList();
        }

        public Requirement FindRequirement(int projectId, int requirementId)
        {
            List<Requirement> everyRequirement = new List<Requirement>();
            FindAllRequirements(projectId, everyRequirement);
            return everyRequirement.FirstOrDefault(item => item.Id == requirementId);
        }

        private void FindAllRequirements(int projectId, List<Requirement> everyRequirement)
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

        #endregion FindRequirements
    }
}