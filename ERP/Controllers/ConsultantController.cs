using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;


namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultantController:ControllerBase
    {

        private readonly IConsultantRepo _consultantRepo;
        private readonly IMapper _mapper;

        public ConsultantController(IConsultantRepo consultantRepo, IMapper mapper)
        {
            _consultantRepo = consultantRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultantReadDto>>> GetAllConsultants()
        {            
            var _consultants = _consultantRepo.GetAllConsultant();            

            return Ok(_mapper.Map<IEnumerable<ConsultantReadDto>>(_consultants));
        }
        [HttpGet("{id}", Name = "GetConsultant")]
        public async Task<ActionResult<ConsultantReadDto>> GetConsultant(string id)
        {            
            var _consultants = _consultantRepo.GetConsultant(id);
            if (_consultants != null)
            {
                return Ok(_mapper.Map<ConsultantReadDto>(_consultants));
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPost]
        public async Task<ActionResult<ConsultantReadDto>> AddConsultant(ConsultantCreateDto consultant)
        {

            var newConsultant = _mapper.Map<Consultant>(consultant);
            _consultantRepo.CreateConsultant(newConsultant);
            _consultantRepo.SaveChanges();            
            var consultantReadDto = _mapper.Map<ConsultantReadDto>(newConsultant);

            return CreatedAtRoute(nameof(GetConsultant), new { id = consultantReadDto }, consultantReadDto);
        }


        
        [HttpDelete("{id}")]
        public async Task<ActionResult<ConsultantReadDto>> DeleteConsultant(string id)
        {
            try
            {
                _consultantRepo.DeleteConsultant(id);
                _consultantRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }





        [HttpPut("{id}")]
        public async Task<ActionResult<ContractReadDto>> UpdateContract(string id, [FromBody] ConsultantCreateDto consultant)
        {
            try
            {

                var newConsultant = _mapper.Map<Consultant>(consultant);
                _consultantRepo.UpdateConsultant(id,newConsultant);
                _consultantRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
















    }
}
