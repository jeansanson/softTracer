using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Repositories;
using System;
using System.Collections.Generic;
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
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody] CreateUserCommand command)
        {
            ValidationError error = new CreateUserCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            UsersRepository repository = new UsersRepository(_connection);
            if (repository.UserExists(command.UserId)) { return BadRequest("Já existe um usuário com este nome."); }
            if (repository.EmailExists(command.Email)) { return BadRequest("Já existe um usuário cadastrado neste e-mail."); }
            repository.CreateUser(command);
            return Ok();
        }

        [HttpPost]
        [Route("authentiation")]
        public IHttpActionResult FindToken([FromBody] FindAuthenticationCommand command)
        {
            ValidationError error = new FindTokenCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            UsersRepository repository = new UsersRepository(_connection);
            Authentication auth = repository.FindAuthentication(command);
            if (auth == null) { return BadRequest("Usuário ou senha inválidos."); }
            return Ok(auth);
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