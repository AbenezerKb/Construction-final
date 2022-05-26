using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;




namespace ERP.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DailyLaborController:ControllerBase
    {


        private readonly IDailyLaborRepo _dailyLaborRepo;
        private readonly IMapper _mapper;

        public DailyLaborController(IDailyLaborRepo dailyLaborRepo, IMapper mapper)
        {
            _dailyLaborRepo = dailyLaborRepo;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyLaborReadDto>>> GetAllDailyLabors()
        {
            var _dailyLabors = _dailyLaborRepo.GetAllDailyLabors();

            return Ok(_mapper.Map<IEnumerable<DailyLaborReadDto>>(_dailyLabors));
        }



        [HttpGet("{id}", Name = "GetDailyLabor")]
        public async Task<ActionResult<DailyLaborReadDto>> GetDailyLabor(string id)
        {

            var _dailyLabors = _dailyLaborRepo.GetDailyLabor(id);
            if (_dailyLabors != null)
            {
                return Ok(_mapper.Map<DailyLaborReadDto>(_dailyLabors));
            }
            else
            {
                return NotFound();
            }


        }





        [HttpPost]
        public async Task<ActionResult<DailyLaborReadDto>> AddDailyLabor(DailyLaborCreateDto dailyLabor)
        {

            var newDailyLabor = _mapper.Map<DailyLabor>(dailyLabor);
            _dailyLaborRepo.CreateDailyLabor(newDailyLabor);
            _dailyLaborRepo.SaveChanges();
            var dailyLaborReadDto = _mapper.Map<DailyLaborReadDto>(newDailyLabor);

            return CreatedAtRoute(nameof(GetDailyLabor), new { id = dailyLaborReadDto.LaborerID }, dailyLaborReadDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<DailyLaborReadDto>> DeleteDailyLabor(string id)
        {

            try
            {
                _dailyLaborRepo.DeleteDailyLabor(id);
                _dailyLaborRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }







        [HttpPut("{id}")]
        public async Task<ActionResult<DailyLaborReadDto>> UpdateDailyLabor(string id, [FromBody] DailyLaborCreateDto dailyLabor)
        {
            try
            {

                var newDailyLabor = _mapper.Map<DailyLabor>(dailyLabor);
                _dailyLaborRepo.UpdateDailyLabor(id,newDailyLabor);
                _dailyLaborRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }












    }


}

