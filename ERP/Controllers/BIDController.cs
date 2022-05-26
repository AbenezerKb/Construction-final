using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;



namespace ERP.Controllers

{
    [Route("api/[controller]")]
    [ApiController]    
    public class BIDController : ControllerBase
    {

        private readonly IBIDRepo _bidRepo;
        private readonly IMapper _mapper;

        public BIDController(IBIDRepo bidRepo, IMapper mapper)
        {
            _bidRepo = bidRepo;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BIDReadDto>>> GetAllBIDs()
        {            
            var _bids = _bidRepo.GetAllBIDs();

            return Ok(_mapper.Map<IEnumerable<BIDReadDto>>(_bids));
        }



        [HttpGet("{id}", Name = "GetBID")]
        public async Task<ActionResult<BIDReadDto>> GetBID(string id)
        {

            var _bids = _bidRepo.GetBID(id);
            if (_bids != null)
            {
                return Ok(_mapper.Map<BIDReadDto>(_bids));
            }
            else
            {
                return NotFound();
            }


        }





        [HttpPost]
        public async Task<ActionResult<BIDReadDto>> AddBID(BIDCreateDto bid)
        {

            var newBid = _mapper.Map<BID>(bid);
            _bidRepo.CreateBID(newBid);
            _bidRepo.SaveChanges();            
            var bidReadDto = _mapper.Map<BIDReadDto>(newBid);

            return CreatedAtRoute(nameof(GetBID), new { id = bidReadDto.BIDID }, bidReadDto);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<BIDReadDto>> DeleteBID(string id)
        {

            try
            {
                _bidRepo.DeleteBID(id);
                _bidRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }






        [HttpPut("{id}")]
        public async Task<ActionResult<BIDReadDto>> UpdateBID(string id, [FromBody] BIDCreateDto bid)
        {
            try
            {

                var newBid = _mapper.Map<BID>(bid);
                _bidRepo.UpdateBID(id,newBid);
                _bidRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }








    }
}



