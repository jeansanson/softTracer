using SofTracerAPI.Controllers;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Misc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Web.Http;

namespace SoftTracerAPI.Controllers
{
    public class UsersController : BaseController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] CreateUserCommand command)
        {
            object a = _connection;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
