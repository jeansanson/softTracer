using MySql.Data.MySqlClient;
using SoftTracerAPI.Misc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public abstract class BaseController : ApiController, IDisposable
    {
        public MySqlConnection _connection { get { return new SoftTracerConnection().connection; } }

        //new void Dispose()
        //{
        //    _connection.Close();
        //}

        //public void Dispose()
        //{
        //    Dispose();
        //}

        // Protected implementation of Dispose pattern.
        new protected virtual void Dispose()
        {
            _connection.Close();
        }

    }

}