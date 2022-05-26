using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;




namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyRequirementController : ControllerBase
    {



        private readonly IWeeklyRequirementRepo _weeklyRequirementRepo;
        private readonly IMapper _mapper;

        public WeeklyRequirementController(IWeeklyRequirementRepo weeklyRequirementRepo, IMapper mapper)
        {
            _weeklyRequirementRepo = weeklyRequirementRepo;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeeklyRequirementReadDto>>> GetAllWeeklyRequirements()
        {
            
            var _weeklyRequirements = _weeklyRequirementRepo.GetAllWeeklyRequirement();

            return Ok(_mapper.Map<IEnumerable<WeeklyRequirementReadDto>>(_weeklyRequirements));
        }


        [HttpGet("{id}", Name = "GetWeeklyRequirement")]
        public async Task<ActionResult<WeeklyRequirementReadDto>> GetWeeklyRequirement(string id)
        {

            var _weeklyRequirement = _weeklyRequirementRepo.GetWeeklyRequirements(id);
            if (_weeklyRequirement != null)
            {
                return Ok(_mapper.Map<WeeklyRequirementReadDto>(_weeklyRequirement));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<WeeklyRequirementReadDto>> AddWeeklyRequirement(WeeklyRequirementCreateDto weeklyRequirement)
        {

            var newWeeklyRequirement = _mapper.Map<WeeklyRequirement>(weeklyRequirement);
            _weeklyRequirementRepo.CreateWeeklyRequirement(newWeeklyRequirement);
            _weeklyRequirementRepo.SaveChanges();
            var weeklyRequirementReadDto = _mapper.Map<WeeklyRequirementReadDto>(newWeeklyRequirement);

            return CreatedAtRoute(nameof(GetWeeklyRequirement), new { id = weeklyRequirementReadDto.Id }, weeklyRequirementReadDto);
        }

        /*
          [Http("{id}", Name = "GetWeeklyRequirement")]
          public async Task<ActionResult<WeeklyRequirementReadDto>> GetWeeklyRequirement(string id)
          {

              var _weeklyRequirement = _weeklyRequirementRepo.GetWeeklyRequirements(id);
              if (_weeklyRequirement != null)
              {
                  return Ok(_mapper.Map<WeeklyRequirementReadDto>(_weeklyRequirement));
              }
              else
              {
                  return NotFound();
              }
          }
        */





        [HttpDelete("{id}")]
        public async Task<ActionResult<WeeklyRequirementReadDto>> DeleteWeeklyRequirement(string id)
        {

            try
            {
                
                    _weeklyRequirementRepo.DeleteWeeklyRequirements(id);
                _weeklyRequirementRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception) 
            {
                return NotFound();
            }
        }





        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWeeklyRequirement(string id, [FromBody] WeeklyRequirementCreateDto weeklyRequirement)
        {
            try
            {

                var newWeeklyRequirement = _mapper.Map<WeeklyRequirement>(weeklyRequirement);
            _weeklyRequirementRepo.UpdateWeeklyRequirement(id,newWeeklyRequirement);
            _weeklyRequirementRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



    }
}
