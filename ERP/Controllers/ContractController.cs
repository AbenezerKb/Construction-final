using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContractRepo _contractRepo;
        private readonly IMapper _mapper;

        public ContractController(IContractRepo contractRepo, IMapper mapper)
        {
            _contractRepo = contractRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<ContractReadDto>>> GetAllContracts()
        {
            
            var _contracts = _contractRepo.GetAllContract();
            
            return Ok( _mapper.Map<IEnumerable<ContractReadDto>>(_contracts));
        }
        [HttpGet("{id}", Name ="GetContract")]
        public async Task <ActionResult<ContractReadDto>> GetContract(string id)
        {            
            var _contracts = _contractRepo.GetContract(id);
            if ( _contracts != null)
            {
                return Ok(_mapper.Map<ContractReadDto>(_contracts));
            }
            else
            {
                return NotFound();
            }

            
        }

        [HttpPost]
        public async Task <ActionResult<ContractReadDto>> AddContract(ContractCreateDto contract)
         {
                     
            var newContract = _mapper.Map<Contract>(contract);
            _contractRepo.CreateContract(newContract);
            _contractRepo.SaveChanges();         
            var contractReadDto = _mapper.Map<ContractReadDto>(newContract);

            return CreatedAtRoute(nameof(GetContract), new { id = contractReadDto.ConId }, contractReadDto);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult<ContractReadDto>> DeleteContract(string id)
        {

            try
            {
                _contractRepo.DeleteContract(id);
                _contractRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }




        [HttpPut("{id}")]
        public async Task<ActionResult<ContractReadDto>> UpdateContract(string id, [FromBody] ContractCreateDto contract)
        {
            try
            {

                var newContract = _mapper.Map<Contract>(contract);
                _contractRepo.UpdateContract(id,newContract);
                _contractRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }









    }
}
