using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;





namespace ERP.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class LaborDetailController: ControllerBase
    {




        private readonly ILaborDetailRepo _laborDetailRepo;
        private readonly IMapper _mapper;

        public LaborDetailController(ILaborDetailRepo laborDetailRepo, IMapper mapper)
        {
            _laborDetailRepo = laborDetailRepo;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LaborDetailReadDto>>> GetAllLaborDetails()
        {
            var _laborDetail = _laborDetailRepo.GetAllLaborDetails();

            return Ok(_mapper.Map<IEnumerable<LaborDetailReadDto>>(_laborDetail));
        }



        [HttpGet("{id}", Name = "GetLaborDetail")]
        public async Task<ActionResult<LaborDetailReadDto>> GetLaborDetail(string id)
        {

            var _laborDetail = _laborDetailRepo.GetLaborDetail(id);
            if (_laborDetail != null)
            {
                return Ok(_mapper.Map<LaborDetailReadDto>(_laborDetail));
            }
            else
            {
                return NotFound();
            }


        }





        [HttpPost]
        public async Task<ActionResult<LaborDetailReadDto>> AddLaborDetail(LaborDetailCreateDto laborDetail)
        {

            var newLaborDetail = _mapper.Map<LaborDetail>(laborDetail);
            _laborDetailRepo.CreateLaborDetail(newLaborDetail);
            _laborDetailRepo.SaveChanges();
            var laborDetailReadDto = _mapper.Map<LaborDetailReadDto>(newLaborDetail);

            return CreatedAtRoute(nameof(GetLaborDetail), new { id = laborDetailReadDto.LaborerID }, laborDetailReadDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<LaborDetailReadDto>> DeleteLaborDetail(string id)
        {

            try
            {
                _laborDetailRepo.DeleteLaborDetails(id);
                _laborDetailRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }








        [HttpPut("{id}")]
        public async Task<ActionResult<LaborDetailReadDto>> UpdateLaborDetail(string id, [FromBody] LaborDetailCreateDto laborDetail)
        {
            try
            {

                var newLaborDetail = _mapper.Map<LaborDetail>(laborDetail);
                _laborDetailRepo.UpdateLaborDetail(id,newLaborDetail);
                _laborDetailRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }




    }
}
