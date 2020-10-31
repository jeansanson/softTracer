using ExtensionMethods;
using MySql.Data.MySqlClient;
using SoftTracerAPI.Commands.Users;
using System;
using System.Data;
using System.Reflection.Emit;
using System.Text;

namespace SoftTracerAPI.Repositories
{
    public class ProjectsRepository

    {
        private readonly  MySqlConnection _connection;

        public ProjectsRepository(MySqlConnection connection)
        {
            _connection = connection;
        }



    }
}