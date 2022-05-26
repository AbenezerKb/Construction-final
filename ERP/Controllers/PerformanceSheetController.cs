using ERP.DTOs;
using ERP.DTOs.Others;
using ERP.Exceptions;
using ERP.Services.PerformanceSheetService;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Controllers
{
    [Route("api/performanceSheets")]
    [ApiController]
    public class PerformanceSheetController : ControllerBase
    {

        private IPerformanceSheetService performanceSheetService;
        public PerformanceSheetController(IPerformanceSheetService sheetService)
        {
            performanceSheetService = sheetService;
        }


        [HttpGet("employees")]
        public async Task<ActionResult<CustomApiResponse>> GetAllEmployeePerformanceSheets([FromQuery] int projectId)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await performanceSheetService.GetAllEmployeePerformanceSheetsByProjectId(projectId)
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
        [HttpGet("subContractors")]
        public async Task<ActionResult<CustomApiResponse>> GetAllSubcontractorPerformanceSheets([FromQuery] int projectId)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await performanceSheetService.GetAllSubcontractorPerformanceSheetsByProjectId(projectId)
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

        [HttpGet("employees/{employeeId:int}")]
        public async Task<ActionResult<CustomApiResponse>> GetEmployeePerformanceSheet(int employeeId, [FromQuery] int projectId)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await performanceSheetService.GetAllByProjectIdAndEmployeeId(employeeId, projectId)
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
        [HttpGet("subContractors/{subContractorId:int}")]
        public async Task<ActionResult<CustomApiResponse>> GetSubContractorPerformanceSheet(int subContractorId, [FromQuery] int projectId)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Data = await performanceSheetService.GetAllByProjectIdAndSubContractorId(subContractorId, projectId)
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




        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CustomApiResponse>> RemovePerformanceSheet(int id)
        {
            try
            {

                return Ok(
                    new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await performanceSheetService.RemoveSheet(id)
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


    }

}