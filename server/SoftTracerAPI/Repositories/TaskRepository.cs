using MySql.Data.MySqlClient;
using System.Text;
using SofTracerAPI.Commands.Tasks;
using System.Threading.Tasks;

namespace SoftTracerAPI.Repositories
{
    public class TaskRepository

    {
        private readonly MySqlConnection _connection;

        public TaskRepository(MySqlConnection connection)
        {
            _connection = connection;
        }

        #region Create

        public void Create(CreateTaskCommand model)
        {
            int taskId = FindNextId(model.ProjectId);
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateQuery();
            PopulateCreateCommand(taskId, model, command);
            if(model.Responsibles != null)
            {
                foreach(string  responsible in model.Responsibles)
                {
                    InsertResponsible(model.ProjectId, taskId, responsible);
                }
            }
            command.ExecuteNonQuery();
        } 

        private static void PopulateCreateCommand(int taskId, CreateTaskCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = model.ProjectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = model.RequirementId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = model.Description;
            command.Parameters.Add("@stage", MySqlDbType.Int32).Value = int.Parse(model.Stage.ToString());
        }

        private string GetCreateQuery()
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO tasks (");
            sql.AppendLine("projectId");
            sql.AppendLine(",requirementId");
            sql.AppendLine(",taskId");
            sql.AppendLine(",name");
            sql.AppendLine(",description");
            sql.AppendLine(",stage)");
            sql.AppendLine("VALUES (");
            sql.AppendLine("@projectId");
            sql.AppendLine(",@requirementId");
            sql.AppendLine(",@taskId");
            sql.AppendLine(",@name");
            sql.AppendLine(",@description");
            sql.AppendLine(",@stage)");
            return sql.ToString();
        }


        private int FindNextId(int projectId)
        {
            MySqlCommand command = new MySqlCommand($"SELECT IFNULL(MAX(taskId) + 1,1) FROM tasks WHERE projectId=@projectId", _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            return int.Parse(command.ExecuteScalar().ToString());
        }

        #endregion Create

        public void InsertResponsible(int projectId, int taskId, string username)
        {
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("INSERT INTO task_responsibles");
            query.AppendLine("(projectId,taskId,user)");
            query.AppendLine("VALUES");
            query.AppendLine("(@projectId,@taskId,user)");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.Parameters.Add("@user", MySqlDbType.Int32).Value = username;
            command.ExecuteNonQuery();
        }

        public void DeleteResponsibles(int projectId, int taskId)
        {
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM task_responsibles");
            query.AppendLine("WHERE projectId=@projectId");
            query.AppendLine("AND taskId=@taskId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.ExecuteNonQuery();
        }


    }
}