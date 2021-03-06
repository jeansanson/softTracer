﻿using SofTracerAPI.Commands;
using SofTracerAPI.Commands.Projects.Requirements;
using SofTracerAPI.Controllers;
using SofTracerAPI.Models.Projects.Requirements;
using SofTracerAPI.Services;
using SoftTracerAPI.Misc;
using SoftTracerAPI.Repositories;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace SoftTracerAPI.Controllers
{
    public class RequirementsController : BaseController
    {
        [TokenAuthenticator]
        [HttpPost]
        [Route("~/api/projects/{projectId:int}/requirements/")]
        public IHttpActionResult CreateRequirements([FromUri] int projectId, [FromBody] List<CreateRequirementsCommand> command)
        {
            if (command == null || projectId <= 0) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new CreateRequirementsCommandValidator().Validate(command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (projectsRepository.Find(projectId) == null) { return BadRequest("Projeto não encontrado."); }
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.Create(projectId, command);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpPut]
        [Route("~/api/projects/{projectId:int}/requirements")]
        public IHttpActionResult CreateUpdateRequirements([FromUri] int projectId, [FromBody] List<UpdateRequirementsCommand> command)
        {
            if (command == null || projectId <= 0) { return BadRequest(DefaultMessages.InvalidBody); }
            ValidationError error = new UpdateRequirementsCommandValidator().Validate( command);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (projectsRepository.Find(projectId) == null) { return BadRequest("Projeto não encontrado."); }
            List<Requirement> requirements = new RequirementsService().MapCommand(command);
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.Update(projectId, requirements);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpPut]
        [Route("~/api/projects/{projectId:int}/requirements/{requirementId}")]
        public IHttpActionResult CreateUpdateRequirement([FromUri] int projectId, [FromBody] UpdateRequirementsCommand command)
        {
            if (command == null || projectId <= 0) { return BadRequest(DefaultMessages.InvalidBody); }
            List<UpdateRequirementsCommand> commands = new List<UpdateRequirementsCommand>() { command };
            ValidationError error = new UpdateRequirementsCommandValidator().Validate(commands);
            if (error.IsInvalid) { return BadRequest(error.Error); }
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (projectsRepository.Find(projectId) == null) { return BadRequest("Projeto não encontrado."); }
            List<Requirement> requirements = new RequirementsService().MapCommand(commands);
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.Update(projectId, requirements);
            return Ok();
        }
        

        [TokenAuthenticator]
        [HttpGet]
        [Route("~/api/projects/{projectId:int}/requirements")]
        public IHttpActionResult FindRequirements([FromUri] int projectId)
        {
            if (projectId <= 0) { return BadRequest(DefaultMessages.InvalidBody); }
            ProjectsRepository projectsRepository = new ProjectsRepository(Connection, HttpContext.Current.User);
            if (projectsRepository.Find(projectId) == null) { return BadRequest("Projeto não encontrado."); }
            RequirementsRepository repository = new RequirementsRepository(Connection);
            return Ok(repository.Find(projectId));
        }

        [TokenAuthenticator]
        [HttpDelete]
        [Route("~/api/projects/{projectId:int}/requirements/{requirementId}")]
        public IHttpActionResult DeleteRequirement([FromUri] int projectId, [FromUri] int requirementId)
        {
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.Delete(projectId, requirementId);
            return Ok();
        }

        [TokenAuthenticator]
        [HttpDelete]
        [Route("~/api/projects/{projectId:int}/requirements")]
        public IHttpActionResult DeleteRequirements([FromUri] int projectId)
        {
            RequirementsRepository repository = new RequirementsRepository(Connection);
            repository.Delete(projectId);
            return Ok();
        }

    }
}