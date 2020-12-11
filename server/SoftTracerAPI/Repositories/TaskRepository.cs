using MySql.Data.MySqlClient;
using System.Text;
using SofTracerAPI.Commands.Tasks;
using System.Data;
using Task = SofTracerAPI.Models.Tasks.Task;
using System.Collections.Generic;

namespace SoftTracerAPI.Repositories
{
    public class TaskRepository

    {
        private readonly MySqlConnection _connection;
        private readonly UsersRepository _usersRepository;

        public TaskRepository(MySqlConnection connection)
        {
            _usersRepository = new UsersRepository(connection);
            _connection = connection;
        }

        #region Update
        public void Update(UpdateTaskCommand model)
        {
            Delete(model.ProjectId, model.TaskId);
            MySqlCommand command = _connection.CreateCommand();
            command.CommandText = GetCreateQuery();
            PopulateUpdateCommand(model, command);
            if (model.Responsibles != null)
            {
                foreach (string responsible in model.Responsibles)
                {
                    InsertResponsible(model.ProjectId, model.TaskId, responsible);
                }
            }
            command.ExecuteNonQuery();
        }

        private static void PopulateUpdateCommand(UpdateTaskCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = model.ProjectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = model.RequirementId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = model.TaskId;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = model.Description;
            command.Parameters.Add("@stage", MySqlDbType.Int32).Value = model.Stage;
        }

        #endregion

        #region Create

        public Task Create(CreateTaskCommand model)
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
            return Find(model.ProjectId, taskId);
        } 

        private static void PopulateCreateCommand(int taskId, CreateTaskCommand model, MySqlCommand command)
        {
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = model.ProjectId;
            command.Parameters.Add("@requirementId", MySqlDbType.Int32).Value = model.RequirementId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = model.Name;
            command.Parameters.Add("@description", MySqlDbType.VarChar).Value = model.Description;
            command.Parameters.Add("@stage", MySqlDbType.Int32).Value = model.Stage;
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

        #region Delete

        public void Delete(int projectId, int taskId)
        {
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM tasks");
            query.AppendLine("WHERE projectId=@projectId");
            query.AppendLine("AND taskId=@taskId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.ExecuteNonQuery();
            DeleteResponsibles(projectId, taskId);
        }

        public void Delete(int projectId)
        {
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM tasks");
            query.AppendLine("WHERE projectId=@projectId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.ExecuteNonQuery();
            DeleteResponsibles(projectId);
        }

        #endregion


        public List<Task> Find(int projectId)
        {
            List<Task> result = new List<Task>();
            StringBuilder query = new StringBuilder(GetFindTaskHeaderQuery());
            query.AppendLine("WHERE projectId=@projectId");
            MySqlCommand command = new MySqlCommand(query.ToString(), _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add(PopulateTask(reader));
                }
            }
            foreach(Task task in result)
            {
                PopulateResponsibles(task);
            }
            return result;
        }

        public Task Find(int projectId, int taskId)
        {
            Task result = null;
            StringBuilder query = new StringBuilder(GetFindTaskHeaderQuery());
            query.AppendLine("WHERE projectId=@projectId");
            query.AppendLine("AND taskId=@taskId");
            MySqlCommand command = new MySqlCommand(query.ToString(), _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            using (IDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    result = PopulateTask(reader);
                }
            }
            PopulateResponsibles(result);
            return result;
        }

        private void PopulateResponsibles(Task task)
        {
            if (task == null) return;
            MySqlCommand command = new MySqlCommand("SELECT user FROM task_responsibles WHERE @projectId=@projectId AND taskId=@taskId", _connection);
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = task.ProjectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = task.Id;
            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    task.Responsibles.Add(reader["user"].ToString());
                }
            }
        }

        private static Task PopulateTask(IDataReader reader)
        {
            return new Task
            {
                Id = int.Parse(reader["taskId"].ToString()),
                ProjectId = int.Parse(reader["projectId"].ToString()),
                Name = reader["name"].ToString(),
                Description = reader["description"].ToString(),
                RequirementId = int.Parse(reader["requirementId"].ToString()),
                Stage = (SofTracerAPI.Models.Tasks.TaskStage)int.Parse(reader["stage"].ToString()),
                Responsibles = new List<string>(),
            };
        }


        private string GetFindTaskHeaderQuery()
        {
            StringBuilder query = new StringBuilder();
            query.AppendLine("SELECT");
            query.AppendLine("taskId");
            query.AppendLine(",projectId");
            query.AppendLine(",requirementId");
            query.AppendLine(",name");
            query.AppendLine(",description");
            query.AppendLine(",stage");
            query.AppendLine("FROM tasks");
            return query.ToString();
        }

        public void InsertResponsible(int projectId, int taskId, string username)
        {
            if (!_usersRepository.UserExists(username)) { return; }
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("INSERT INTO task_responsibles");
            query.AppendLine("(projectId,taskId,user)");
            query.AppendLine("VALUES");
            query.AppendLine("(@projectId,@taskId,@user)");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.Parameters.Add("@taskId", MySqlDbType.Int32).Value = taskId;
            command.Parameters.Add("@user", MySqlDbType.VarChar).Value = username;
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

        public void DeleteResponsibles(int projectId)
        {
            MySqlCommand command = _connection.CreateCommand();
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM task_responsibles");
            query.AppendLine("WHERE projectId=@projectId");
            command.CommandText = query.ToString();
            command.Parameters.Add("@projectId", MySqlDbType.Int32).Value = projectId;
            command.ExecuteNonQuery();
        }


    }
}