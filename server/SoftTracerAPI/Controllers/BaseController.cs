using MySql.Data.MySqlClient;
using SoftTracerAPI.Misc;
using System;
using System.Web.Http;

namespace SofTracerAPI.Controllers
{
    public abstract class BaseController : ApiController, IDisposable
    {
        public MySqlConnection Connection { get { return new SoftTracerConnection().Connection; } }
        public DefaultMessages DefaultMessages { get { return new DefaultMessages(); } }

        // Protected implementation of Dispose pattern.
        new protected virtual void Dispose()
        {
            Connection.Close();
        }
    }
}