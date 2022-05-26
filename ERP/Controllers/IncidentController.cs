
namespace ERP.Controllers
{
   
using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class IncidentController:ControllerBase
    {
        private readonly IIncidentRepo _incidentRepo;
        private readonly IMapper _mapper;

        public IncidentController(IIncidentRepo incidentRepo, IMapper mapper)
        {
            _incidentRepo = incidentRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncidentReadDto>>> GetAllIncidents()
        {
            Console.WriteLine("....started");
            var _incidents = _incidentRepo.GetAllIncidents();
            Console.WriteLine("....started", _incidents);

            return Ok(_mapper.Map<IEnumerable<IncidentReadDto>>(_incidents));
        }
        [HttpGet("{id}", Name = "GetIncident")]
        public async Task<ActionResult<IncidentReadDto>> GetIncident(string id)
        {
            Console.WriteLine("....started");
            var _incidents = _incidentRepo.GetIncident(id);
            
            if (_incidents != null)
            {
                return Ok(_mapper.Map<IncidentReadDto>(_incidents));
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPost]
        public async Task<ActionResult<IncidentReadDto>> AddIncident(IncidentCreateDto incident)
        {

            var newIncident = _mapper.Map<Incident>(incident);
            _incidentRepo.CreateIncident(newIncident);
            _incidentRepo.SaveChanges();
            Console.WriteLine("the incident id: " + newIncident.incidentNo);
            var incidentReadDto = _mapper.Map<IncidentReadDto>(newIncident);

            return CreatedAtRoute(nameof(GetIncident), new { id = incidentReadDto.incidentNo }, incidentReadDto);
        }



        
        [HttpDelete("{id}")]
        public async Task<ActionResult<IncidentReadDto>> DeleteIncident(string id)
        {

            try
            {
                _incidentRepo.DeleteIncident(id);
                _incidentRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
 }
}
