using AutoMapper;
using ERP.DTOs;
using ERP.Models;
using ERP.Services;
using Microsoft.AspNetCore.Mvc;


namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeCardController: ControllerBase
    {
        private readonly ITimeCardRepo _timeCardRepo;
        private readonly IMapper _mapper;

        public TimeCardController(ITimeCardRepo timeCardRepo, IMapper mapper)
        {
            _timeCardRepo = timeCardRepo;
            _mapper = mapper;

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeCardReadDto>>> GetAllTimeCards()
        {
            Console.WriteLine("....started");
            var _timeCards = _timeCardRepo.GetAllTimeCards();
            Console.WriteLine("....started", _timeCards);

            return Ok(_mapper.Map<IEnumerable<TimeCardReadDto>>(_timeCards));
        }


        [HttpGet("{id}", Name = "GetTimeCard")]
        public async Task<ActionResult<TimeCardReadDto>> GetTimeCard(string id)
        {
            Console.WriteLine("....started");
            var _timeCard = _timeCardRepo.GetTimeCard(id);
            if (_timeCard != null)
            {
                return Ok(_mapper.Map<TimeCardReadDto>(_timeCard));
            }
            else
            {
                return NotFound();
            }
        }

            [HttpPost]
            public async Task<ActionResult<TimeCardReadDto>> AddTimeCard(TimeCardCreateDto timeCard)
            {

                var newTimeCard = _mapper.Map<TimeCard>(timeCard);
                _timeCardRepo.CreateTimeCard(newTimeCard);
                _timeCardRepo.SaveChanges();                
                var timeCardReadDto = _mapper.Map<TimeCardReadDto>(newTimeCard);

                return CreatedAtRoute(nameof(GetTimeCard), new { id = timeCardReadDto.Id }, timeCardReadDto);
            }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TimeCardReadDto>> DeleteTimeCard(string id)
        {

            try
            {
                _timeCardRepo.DeleteTimeCard(id);
                _timeCardRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }





        [HttpPut("{id}")]
        public async Task<ActionResult<TimeCardReadDto>> UpdateTimeCard(string id, [FromBody] TimeCardCreateDto timeCard)
        {
            try
            {

                var newTimeCard = _mapper.Map<TimeCard>(timeCard);
                _timeCardRepo.UpdateTimeCard(id, newTimeCard);
                _timeCardRepo.SaveChanges();
                return Ok("Success");
            }
            catch (Exception)
            {
                return NotFound();
            }
        }






    }
}

