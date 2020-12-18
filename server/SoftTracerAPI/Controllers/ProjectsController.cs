using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SofTracerAPI.Models.Projects;
using SoftTracerAPI.Commands.Projects;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Models;
using SoftTracerAPI.Repositories;
using System.Web;
using System.Web.Http;

namespace SoftTracerAPI.Controllers
{
    public class ProjectsController : BaseController
    {
        [TokenAuthenticator]
        [HttpGet]
        public IHttpActionResult FindProjects()
        {
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            return Ok(repository.Find());
        }

        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}")]
        public IHttpActionResult FindProjectById([FromUri] int projectId)
        {
            if (projectId <= 0) { return BadRequest("Id inválido"); }
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            return Ok(repository.Find(projectId));
        }

        [TokenAuthenticator]
        [HttpPost]
        public IHttpActionResult CreateProject([FromBody] CreateProjectCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateProjectCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            return Ok(repository.Create(command));
        }

        [TokenAuthenticator]
        [HttpPut]
        [Route("~/api/projects/{projectId:int}")]
        public IHttpActionResult UpdateProject([FromBody] UpdateProjectCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new UpdateProjectCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (repository.Find(command.ProjectId) == null) { return BadRequest("Projeto não encontrado"); }
            repository.Update(command);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpPost]
        [Route("~/api/projects/users")]
        public IHttpActionResult AddUser([FromBody] InsertUserCommand command)
        {
            ValidationError error = new InsertUserCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            UsersRepository usersRepository = new UsersRepository(Connection);
            if (!usersRepository.UserExists(User.Identity.Name))
            {
                return BadRequest("Usuário não encontrado");
            }

            ProjectsRepository repository = new ProjectsRepository(Connection, User);
            repository.AddUser(command.ProjectToken, UserRole.Guest);
            return Ok(repository.Find(command.ProjectToken));
        }

        [TokenAuthenticator]
        [HttpDelete]
        [Route("~/api/projects/{projectId:int}/users/{username}")]
        public IHttpActionResult DeleteUser([FromUri] int projectId, [FromUri] string username)
        {
            ProjectsRepository repository = new ProjectsRepository(Connection, User);
            if (repository.Find(projectId) == null) { return BadRequest("Projeto não encontrado"); }
            repository.DeleteUser(projectId, username);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}/users")]
        public IHttpActionResult GetUsers([FromUri] int projectId)
        {
            ProjectsRepository repository = new ProjectsRepository(Connection, User);
            if (repository.Find(projectId) == null) { return BadRequest("Projeto não encontrado"); }

            UsersRepository usersRepository = new UsersRepository(Connection);
            return Ok(usersRepository.FindUsers(projectId));
        }

        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}/users/{username}")]
        public IHttpActionResult GetUser([FromUri] int projectId, [FromUri] string username)
        {
            ProjectsRepository repository = new ProjectsRepository(Connection, User);
            if (repository.Find(projectId) == null) { return BadRequest("Projeto não encontrado"); }

            UsersRepository usersRepository = new UsersRepository(Connection);
            ProjectUser user = usersRepository.FindUser(projectId, username);
            if (user == null) { return NotFound(); }
            return Ok(user);
        }
    }
}