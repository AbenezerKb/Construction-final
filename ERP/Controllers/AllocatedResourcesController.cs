using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;
namespace ERP.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AllocatedResourcesController : ControllerBase
    {
        private readonly IAllocatedResourcesRepo _allocatedResourcesRepo;
        private readonly IMapper _mapper;

        public AllocatedResourcesController(IAllocatedResourcesRepo allocatedResourcesRepo, IMapper mapper)
        {
            _allocatedResourcesRepo = allocatedResourcesRepo;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllocatedResourcesReadDto>>> GetAllAssignedWorkForces()
        {
            var _allocatedResources = _allocatedResourcesRepo.GetAllAllocatedResources();

            return Ok(_mapper.Map<IEnumerable<AllocatedResourcesReadDto>>(_allocatedResources));
        }



        [HttpGet("{id}", Name = "GetAllocatedResources")]
        public async Task<ActionResult<AllocatedResourcesReadDto>> GetAllocatedResources(string id)
        {

            var _allocatedResources = _allocatedResourcesRepo.GetAllocatedResources(id);
            if (_allocatedResources != null)
            {
                return Ok(_mapper.Map<AllocatedResourcesReadDto>(_allocatedResources));
            }
            else
            {
                return NotFound();
            }


        }



        [HttpPost]
        public async Task<ActionResult<AllocatedResourcesReadDto>> AddAllocatedResources(AllocatedResourcesCreateDto allocatedResources)
        {

            var newAllocatedResources = _mapper.Map<AllocatedResources>(allocatedResources);
            _allocatedResourcesRepo.CreateAllocatedResources(newAllocatedResources);
            _allocatedResourcesRepo.SaveChanges();
            var allocatedResourcesReadDto = _mapper.Map<AllocatedResourcesReadDto>(newAllocatedResources);

            return CreatedAtRoute(nameof(GetAllocatedResources), new { id = allocatedResourcesReadDto.allocatedResourcesNo  }, allocatedResourcesReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AllocatedResourcesReadDto>> DeleteBID(string id)
        {

            try
            {
                _allocatedResourcesRepo.DeleteAllocatedResource(id);
                _allocatedResourcesRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }



        [HttpPut("{id}")]
        public async Task<ActionResult<AllocatedResourcesReadDto>> UpdateAllocatedResource(string id, [FromBody] AllocatedResourcesCreateDto allocatedResource)
        {
            try
            {
                var newAllocatedResources = _mapper.Map<AllocatedResources>(allocatedResource);
            _allocatedResourcesRepo.UpdateAllocatedResource(id,newAllocatedResources);
            _allocatedResourcesRepo.SaveChanges();
            return Ok("Success");
        }
            catch (Exception)
            {
                return NotFound();
    }
}


    }
}
