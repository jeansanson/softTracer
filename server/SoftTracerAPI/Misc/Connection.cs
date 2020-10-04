using System.Data.SqlClient;


namespace SofTracerAPI.Misc
{
    public class SoftTracerConnection
    {
        public readonly SqlConnection connection = new SqlConnection("Server = root@localhost:4000; Database=softracer; Uid=movtech;Pwd=mvt;MultipleActiveResultSets=True");

        public SoftTracerConnection()
        {
            connection.Open();
        }

        public SqlCommand GetCommand()
        {
            SqlCommand command = connection.CreateCommand();
            command.Connection = connection;
            return command;

        }

    }
}