using MySql.Data.MySqlClient;


namespace SoftTracerAPI.Misc
{
    public class SoftTracerConnection
    {
        public readonly MySqlConnection Connection = new MySqlConnection("Server=localhost; Port=4000; Database=softracer; Uid=root; Pwd=123456;");

        public SoftTracerConnection()
        {
            Connection.Open();
        }


    }
}