using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
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
            return Ok(repository.CreateProject(command));
        }

        [TokenAuthenticator]
        [HttpGet]
        public IHttpActionResult FindProjects()
        {
            ProjectsRepository repository = new ProjectsRepository(Connection, HttpContext.Current.User);
            return Ok(repository.FindProjects());
        }

    }
}
