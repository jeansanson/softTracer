using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Repositories;
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
        public IHttpActionResult CreateUser([FromBody] CreateUserCommand command)
        {
            ValidationError error = new CreateUserCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error);}
            UsersRepository repository = new UsersRepository(_connection);
            if (repository.UserExists(command.UserId)) { return BadRequest("Já existe um usuário com este nome."); }
            if (repository.EmailExists(command.Email)) { return BadRequest("Já existe um usuário cadastrado neste e-mail."); }
            repository.CreateUser(command);
            return Ok();
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
