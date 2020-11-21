using SofTracerAPI.Commands;
using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Controllers;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Repositories;
using System.Web;
using System.Web.Http;

namespace SoftTracerAPI.Controllers
{
    public class RequirementsController : BaseController
    {
        [HttpPost]
        public IHttpActionResult CreateUpdateRequirements([FromBody] CreateRequirementsCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateRequirementsCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (projectsRepository.FindProject(command.ProjectId) == null) { return BadRequest("Projeto não encontrado."); }
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.CreateRequirements(command);
            return Ok();
        }
    }
}