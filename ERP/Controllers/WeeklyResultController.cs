using ERP.DTOs.Others;
using ERP.DTOs.WeeklyResult;
using ERP.Exceptions;
using ERP.Services.WeeklyResultService;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Controllers
{
    [ApiController]
    [Route("api/weeklyResults")]
    public class WeeklyResultController : ControllerBase
    {
        private readonly IWeeklyResultService weeklyResultService;
        public WeeklyResultController(IWeeklyResultService service)
        {
            weeklyResultService = service;
        }
        [HttpPost]
        public async Task<ActionResult<CustomApiResponse>> AddWeeklyResult([FromBody] WeeklyResultDto weeklyResultDto)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await weeklyResultService.Add(weeklyResultDto)
                });

            }
            catch (ItemAlreadyExistException iaex)
            {
                return Conflict(new CustomApiResponse
                {
                    Message = iaex.Message
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

        [HttpGet("{weeklyResultId:int}")]
        public async Task<ActionResult<CustomApiResponse>> GetWeeklyResultById(int weeklyResultId)
        {
            try
            {
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await weeklyResultService.GetById(weeklyResultId)
                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(
                    new CustomApiResponse
                    {
                        Message = infe.Message
                    }
                );
            }
        }
        [HttpGet]
        public async Task<ActionResult<CustomApiResponse>> GetWeeklyResultsBy([FromQuery] int? projectId, [FromQuery] int? weeklyPlanId)
        {
            try
            {

                if (projectId != null && weeklyPlanId != null)
                {
                    return BadRequest(
                        new CustomApiResponse
                        {
                            Message = "Invalid Request, Filtering with morethan one parameter is not allowed!"
                        }
                    );
                }
                if (projectId != null)
                {
                    return Ok(new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await weeklyResultService.GetByProjectId(projectId.Value)
                    });
                }
                else if (weeklyPlanId != null)
                {
                    return Ok(new CustomApiResponse
                    {
                        Message = "Success",
                        Data = await weeklyResultService.GetByWeeklyPlanId(weeklyPlanId.Value)
                    });
                }
                else
                {
                    return Ok(new CustomApiResponse
                    {
                        Data = await weeklyResultService.GetAll()
                    });
                }

            }
            catch (ItemNotFoundException infex)
            {
                return NotFound(new CustomApiResponse { Message = infex.Message });
            }
        }
        [HttpPut("{weeklyResultId:int}/Values/{weeklyResultValueId:int}")]
        public async Task<ActionResult<CustomApiResponse>> UpdateWeeklyResult(int weeklyResultId, int weeklyResultValueId, [FromBody] int newValue)
        {
            try
            {
                if (newValue < 0 || newValue > 100)
                {
                    return BadRequest(
                        new CustomApiResponse
                        {
                            Message = $"{newValue} is invalid value, please specify a number between 0-100"
                        }
                    );
                }

                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await weeklyResultService.UpdateResult(weeklyResultId, newValue)
                });
            }
            catch (ItemNotFoundException infex)
            {
                return NotFound(new CustomApiResponse { Message = infex.Message });
            }
        }
        [HttpDelete("{weeklyResultId}")]
        public async Task<ActionResult<CustomApiResponse>> RemoveWeeklyResult(int weeklyResultId)
        {

            try
            {
                return Ok(new CustomApiResponse
                {
                    Message = "Success",
                    Data = await weeklyResultService.Remove(weeklyResultId)
                });
            }
            catch (ItemNotFoundException infe)
            {
                return NotFound(new CustomApiResponse { Message = infe.Message });

            }

        }

      
    }

}