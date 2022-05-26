using Microsoft.AspNetCore.Mvc;
using ERP.Services.ProjectService;
using ERP.Services.ProjectManagementReportService;
using ERP.Services.ProjectManagementAnalyticsService;
using ERP.DTOs.Others;
using ERP.DTOs.Project;
using ERP.Exceptions;
using ERP.Models;
using ERP.Helpers;
using ERP.Services.ProjectTaskService;

namespace ERP.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        private readonly IProjectTaskService projectTaskService;
        private readonly IProjectManagementReportService projectManagementReportService;
        private readonly IProjectManagementAnalyticsService projectManagementAnalyticsService;
        public ProjectController(IProjectService service,
         IProjectManagementReportService projectManagementReportService,
          IProjectManagementAnalyticsService projectManagementAnalyticsService, IProjectTaskService projectTaskService)
        {
            projectService = service;
            this.projectManagementReportService = projectManagementReportService;
            this.projectManagementAnalyticsService = projectManagementAnalyticsService;
            this.projectTaskService = projectTaskService;
        }


        [HttpPost]
        async public Task<ActionResult<CustomApiResponse>> AddProject([FromBody] ProjectDto projectDto)
        {

            if (!Utils.isValidDateRange(projectDto.StartDate, projectDto.EndDate))
            {
                return BadRequest(new CustomApiResponse
                {
                    Message = "Invalid Date Range, StartDate must be less than EndDate and EndDate must be greater than StartDate",

                });
            }
            return Ok(new CustomApiResponse
            {
                Data = await projectService.Add(projectDto),
                Message = "Success"
            });

        }
        [HttpGet]
        public async Task<ActionResult<CustomApiResponse>> GetProjects([FromQuery] int? siteId, [FromQuery] string? name)
        {
            try
            {
                List<Project> projects = await FilterProjectsBy(name, siteId);

                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = projects
                });

            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {
                    Message = infe.Message
                });

            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomApiResponse>> GetProjectById(int id)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await projectService.GetById(id),
                    Message = "Success"
                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse { Message = infe.Message });
            }
        }

        [HttpGet("{id:int}/subContractorWorks")]

        public async Task<ActionResult<ProjectTask>> GetSubContractingWorks(int id)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await projectTaskService.GetSubContractorWorks(id),
                    Message = "Success"
                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse { Message = infe.Message });
            }

        }

        [HttpGet("{id:int}/report")]

        public async Task<ActionResult<CustomApiResponse>> GetReport(int id, [FromQuery] DateTime StartDate, [FromQuery] DateTime EndDate)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await projectManagementReportService.GetReportWith(StartDate, EndDate, id)

                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {

                    Message = infe.Message
                });

            }
        }
        [HttpGet("{id:int}/analytics")]

        public async Task<ActionResult<CustomApiResponse>> GetAnalytics(int id)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await projectManagementAnalyticsService.GetAnalytics(id)

                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {

                    Message = infe.Message
                });

            }
        }
        [HttpGet("{id:int}/taskProgressSheet")]
        public async Task<ActionResult<CustomApiResponse>> GetTaskProgressSheet(int id)
        {
            try
            {
                return Ok(
                    new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await projectService.GetTaskProgressSheet(id)
                    }
                );
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {
                    Message = infe.Message
                });
            }
        }
        [HttpGet("{id:int}/crashSchedule")]
        public async Task<ActionResult<CustomApiResponse>> GetCrashSchedule(int id)
        {
            try
            {
                return Ok(
                    new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await projectService.GetCrashSchedule(id)
                    }
                );
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {
                    Message = infe.Message
                });
            }

        }
        [HttpGet("{id:int}/actualSchedule")]
        public async Task<ActionResult<CustomApiResponse>> GetActualSchedule(int id)
        {
            try
            {
                return Ok(
                    new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await projectService.GetActualSchedule(id)
                    }
                );
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse
                {
                    Message = infe.Message
                });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CustomApiResponse>> DeleteProject(int id)
        {

            try
            {
                Project project = await projectService.Remove(id);
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = project
                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse { Message = infe.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CustomApiResponse>> UpdateProject(int id, [FromBody] ProjectDto projectDto)
        {

            try
            {
                Project project = await projectService.Update(id, projectDto);

                return Ok(new CustomApiResponse
                {
                    Data = project,
                    Message = "Success"
                });

            }
            catch (ItemNotFoundException infe)
            {

                return NotFound(new CustomApiResponse { Message = infe.Message });
            }

        }


        private async Task<List<Project>> FilterProjectsBy(string? name, int? siteId)
        {
            List<Project> projects = new List<Project>();
            if (siteId != null && name != null)
            {
                projects = await projectService.GetByNameAndSiteId(name, siteId.Value);
            }
            else if (siteId != null)
            {
                projects = await projectService.GetBySiteId(siteId.Value);

            }
            else if (name != null)
            {
                projects = await projectService.GetByName(name);
            }
            else
            {
                projects = await projectService.GetAll();
            }
            return projects;
        }
    }
}
