using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubContractorController: ControllerBase
    {



        private readonly ISubContractorRepo _subcontractRepo;
        private readonly IMapper _mapper;

        public SubContractorController(ISubContractorRepo subcontractRepo, IMapper mapper)
        {
            _subcontractRepo = subcontractRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubContractorReadDto>>> GetAllSubContractors()
        {
            
            var _subcontracts = _subcontractRepo.GetAllSubContractors();            

            return Ok(_mapper.Map<IEnumerable<SubContractorReadDto>>(_subcontracts));
        }
        [HttpGet("{id}", Name = "GetSubContractor")]
        public async Task<ActionResult<SubContractorReadDto>> GetSubContractor(string id)
        {
           
            var _subcontracts = _subcontractRepo.GetSubContractor(id);
            if (_subcontracts != null)
            {
                return Ok(_mapper.Map<SubContractorReadDto>(_subcontracts));
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPost]
        public async Task<ActionResult<SubContractorReadDto>> AddSubContractor(SubContractorCreateDto contract)
        {

            var newContract = _mapper.Map<SubContractor>(contract);
            _subcontractRepo.CreateSubContractor(newContract);
            _subcontractRepo.SaveChanges();
         
            var subcontractReadDto = _mapper.Map<SubContractorReadDto>(newContract);

            return CreatedAtRoute(nameof(GetSubContractor), new { id = subcontractReadDto.SubId }, subcontractReadDto);
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<SubContractorReadDto>> DeleteSubContractor(string id)
        {

            try
            {
                _subcontractRepo.DeleteSubContractor(id);
                _subcontractRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }








        [HttpPut("{id}")]
        public async Task<ActionResult<SubContractorReadDto>> UpdateSubContractor(string id, [FromBody] SubContractorCreateDto subContractor)
        {
            try
            {

                var newSubContractor = _mapper.Map<SubContractor>(subContractor);
                _subcontractRepo.UpdateSubContractor(id, newSubContractor);
                _subcontractRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }


    }
}
