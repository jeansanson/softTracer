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
using SofTracerAPI.Models.Tasks;

namespace SoftTracerAPI.Controllers
{
    public class TasksController : BaseController
    {
        [TokenAuthenticator]
        [HttpPost]
        [Route("~/api/projects/{projectId:int}/tasks/")]
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

            TaskRepository taskRepository = new TaskRepository(Connection);
            return Ok(taskRepository.Create(command));
        }

         
        [TokenAuthenticator]
        [HttpPut]
        [Route("~/api/projects/{projectId:int}/tasks/{taskId:int}")]
        public IHttpActionResult UpdateTask([FromBody] UpdateTaskCommand command)
        {
            if (command == null) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new UpdateTaskCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }

            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(command.ProjectId);
            if (project == null) { return BadRequest("Projeto não encontrado."); }

            if (command.RequirementId > 0)
            {
                RequirementsRepository requirementsRepository = new RequirementsRepository(Connection);
                Requirement requirement = requirementsRepository.Find(command.ProjectId, command.RequirementId);
                if (requirement == null) { return BadRequest("Requisito não encontrado neste projeto."); }
            }

            TaskRepository taskRepository = new TaskRepository(Connection);
            taskRepository.Update(command);
            return Ok();
        }


        [TokenAuthenticator]
        [HttpDelete]
        [Route("~/api/projects/{projectId:int}/tasks/")]
        public IHttpActionResult Delete([FromUri] int projectId)
        {
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(projectId);
            if (project == null) { return BadRequest("Projeto não encontrado."); }

            TaskRepository taskRepository = new TaskRepository(Connection);
            taskRepository.Delete(projectId);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpDelete]
        [Route("~/api/projects/{projectId:int}/tasks/{taskId:int}")]
        public IHttpActionResult DeleteBYId([FromUri] int projectId, [FromUri] int taskId)
        {
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(projectId);
            if (project == null) { return BadRequest("Projeto não encontrado."); }

            TaskRepository taskRepository = new TaskRepository(Connection);
            taskRepository.Delete(projectId, taskId);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}/tasks/")]
        public IHttpActionResult FindAll([FromUri] int projectId)
        {
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(projectId);
            if (project == null) { return BadRequest("Projeto não encontrado."); }

            TaskRepository taskRepository = new TaskRepository(Connection);
            return Ok(taskRepository.Find(projectId));
        }



        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}/tasks/{taskId:int}")]
        public IHttpActionResult Find([FromUri] int projectId, [FromUri] int taskId)
        {
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, User);
            Project project = projectsRepository.Find(projectId);
            if (project == null) { return BadRequest("Projeto não encontrado."); }

            TaskRepository taskRepository = new TaskRepository(Connection);
            Task task = taskRepository.Find(projectId, taskId);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }


    }
}