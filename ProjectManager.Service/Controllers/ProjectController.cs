namespace ProjectManager.Service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using ProjectManager.Service.Business;
    using ProjectManager.Service.Models;

    [Produces("application/json")]
    [Route("api/Project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectManager projectManager, ILogger<ProjectController> logger)
        {
            _projectManager = projectManager;
            _logger = logger;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            try
            {
                _logger.LogInformation("Fetching Available project Details");
                return Ok(await _projectManager.GetAllProjects());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service, Please contact service team");
            }
        }

        // GET: api/Project/5
        [HttpGet("{id}", Name = "GetProject")]
        public async Task<IActionResult> GetProjectbyId(int id)
        {
            try
            {
                _logger.LogInformation($"Fetching details for project {id}");
                return Ok(await _projectManager.GetProjectDetail(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service while fetching project details, Please contact service team");
            }
        }

        // POST: api/Project
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectDetailModel project)
        {
            try
            {
                if (project == null)
                {
                    _logger.LogInformation("Invalid project item detail.");
                    return BadRequest();
                }

                await _projectManager.AddProjectDetails(project);

                _logger.LogInformation($"Inserted project to database with id {project.ProjectId}");

                return Ok($"Project with id {project.ProjectId} created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service while inserting task, Please contact service team");
            }
        }

        // PUT: api/Project/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectDetailModel projectDetailModel)
        {
            try
            {
                if (projectDetailModel == null || projectDetailModel.ProjectId != id)
                {
                    _logger.LogInformation("Provide valid project item detail");
                    return BadRequest("Invalid project detail");
                }

                if (!_projectManager.IsProjectValid(projectDetailModel))
                {
                    return BadRequest("This project has active tasks. Active tasks has to be closed before closing project");
                }

                await _projectManager.UpdateProjectDetails(id, projectDetailModel);
                _logger.LogInformation($"Updated project with id  {projectDetailModel.ProjectId}");

                return Ok($"{projectDetailModel.ProjectName} Saved");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An exception has occured with Service, Please contact service team");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var project = await _projectManager.GetProjectDetail(id);
                if (!_projectManager.IsProjectValid(project))
                {
                    _logger.LogInformation("You can not delete as the project have association with Task");
                    return BadRequest("You can not delete as the project have association with Task");
                }

                await _projectManager.RemoveProject(project);

                return Ok(project.ProjectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server error. Try again later");
            }

        }
    }
}
