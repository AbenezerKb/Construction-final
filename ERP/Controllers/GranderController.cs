using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GranderController: ControllerBase
    {

        private readonly IGranderRepo _granderRepo;
        private readonly IMapper _mapper;

        public GranderController(IGranderRepo granderRepo, IMapper mapper)
        {
            _granderRepo = granderRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GranderReadDto>>> GetAllGranders()
        {
            
            var _granders = _granderRepo.GetAllGranders();
          

            return Ok(_mapper.Map<IEnumerable<GranderReadDto>>(_granders));
        }
        [HttpGet("{id}", Name = "GetGrander")]
        public async Task<ActionResult<GranderReadDto>> GetGrander(string id)
        {            
            var _granders = _granderRepo.GetGrander(id);
            if (_granders != null)
            {
                return Ok(_mapper.Map<GranderReadDto>(_granders));
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPost]
        public async Task<ActionResult<GranderReadDto>> AddGrander(GranderCreateDto grander)
        {

            var newGrander = _mapper.Map<Grander>(grander);
            _granderRepo.CreateGrander(newGrander);
            _granderRepo.SaveChanges();
            
            var granderReadDto = _mapper.Map<GranderReadDto>(newGrander);

            return CreatedAtRoute(nameof(GetGrander), new { id = granderReadDto.GranderId }, granderReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<GranderReadDto>> DeleteGrander(string id)
        {

            try
            {
                _granderRepo.DeleteGrander(id);
                _granderRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }




    }
}
