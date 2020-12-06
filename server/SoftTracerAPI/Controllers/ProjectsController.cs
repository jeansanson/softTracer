using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SofTracerAPI.Models.Projects;
using SoftTracerAPI.Commands.Projects;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Repositories;
using System.Web;
using System.Web.Http;

namespace SoftTracerAPI.Controllers
{
    public class ProjectsController : BaseController
    {
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
            if(projectId <= 0) { return BadRequest("Id inválido"); }
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            return Ok(repository.Find(projectId));
        }

        [TokenAuthenticator]
        [HttpPost]
        [Route("~/api/projects/users")]
        public IHttpActionResult InsertUser([FromBody] InsertUserCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new InsertUserCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            UsersRepository usersRepository = new UsersRepository(Connection);
            if (!usersRepository.UserExists(command.UserId)) {
                return BadRequest("Usuário não encontrado");
            }
          
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            if(projectsRepository.Find(command.ProjectToken) == null)
            {
                return BadRequest("Projeto não encontrado");
            }

            projectsRepository.AddUser(command.ProjectToken, command.UserId, UserRole.Guest);
            return Ok();
        }

    }
}
