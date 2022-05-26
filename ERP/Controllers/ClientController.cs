using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController:ControllerBase
    {


        private readonly IClientRepo _clientRepo;
        private readonly IMapper _mapper;

        public ClientController(IClientRepo clientRepo, IMapper mapper)
        {
            _clientRepo = clientRepo;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetAllClients()
        {
            var _clients = _clientRepo.GetAllClient();

            return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(_clients));
        }
        [HttpGet("{id}", Name = "GetClient")]
        public async Task<ActionResult<ClientReadDto>> GetClient(string id)
        {
            var _clients = _clientRepo.GetClient(id);
            if (_clients != null)
            {
                return Ok(_mapper.Map<ClientReadDto>(_clients));
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPost]
        public async Task<ActionResult<ClientReadDto>> AddClient(ClientCreateDto client)
        {

            var newClient = _mapper.Map<Client>(client);
            _clientRepo.CreateClient(newClient);
            _clientRepo.SaveChanges();
            var clientReadDto = _mapper.Map<ClientReadDto>(newClient);

            return CreatedAtRoute(nameof(GetClient), new { id = clientReadDto }, clientReadDto);
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientReadDto>> DeleteClient(string id)
        {

            try
            {
                _clientRepo.DeleteClient(id);
                _clientRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }







        [HttpPut("{id}")]
        public async Task<ActionResult<ClientReadDto>> UpdateClient(string id, [FromBody] ClientCreateDto client)
        {
            try
            {

                var newClient = _mapper.Map<Client>(client);
                _clientRepo.UpdateClient(id,newClient);
                _clientRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }








    }
}
