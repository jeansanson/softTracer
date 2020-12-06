using SofTracerAPI.Commands;
using SofTracerAPI.Controllers;
using SoftTracerAPI.Commands.Users;
using SoftTracerAPI.Repositories;
using SoftTracerAPI.Models;
using System.Web.Http;
using SoftTracerAPI.Misc;
using SofTracerAPI.Commands.Tasks;
using SofTracerAPI.Models.Projects;
using SofTracerAPI.Models.Projects.Requirements;

namespace SoftTracerAPI.Controllers
{
    public class TasksController : BaseController
    {
        [TokenAuthenticator]
        [HttpPost]
        public IHttpActionResult CreateTask([FromBody] CreateTaskCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateTaskCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(command.ProjectId);
            if(project == null) { return BadRequest("Projeto não encontrado."); }

            if(command.RequirementId > 0)
            {
                RequirementsRepository requirementsRepository = new RequirementsRepository(Connection);
                Requirement requirement = requirementsRepository.Find(command.ProjectId, command.RequirementId);
                if(requirement == null ) { return BadRequest("Requisito não encontrado neste projeto."); }
            }
      


            return Ok();
        }



    }
}