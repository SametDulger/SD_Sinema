using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;
using SD_Sinema.Core.Entities;
using SD_Sinema.Core.Interfaces;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ISeatTypeService _seatTypeService;

        public SeatsController(ISeatService seatService, ISeatTypeService seatTypeService)
        {
            _seatService = seatService;
            _seatTypeService = seatTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetAll()
        {
            var seats = await _seatService.GetAllAsync();
            return Ok(seats);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetActive()
        {
            var seats = await _seatService.GetActiveSeatsAsync();
            return Ok(seats);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<string>>> GetSeatTypes()
        {
            var seatTypeNames = await _seatTypeService.GetActiveSeatTypeNamesAsync();
            return Ok(seatTypeNames);
        }

        [HttpGet("salon/{salonId}")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetBySalon(int salonId)
        {
            var seats = await _seatService.GetSeatsBySalonAsync(salonId);
            return Ok(seats);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeatDto>> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);
            if (seat == null)
                return NotFound();

            return Ok(seat);
        }

        [HttpPost]
        public async Task<ActionResult<SeatDto>> Create(CreateSeatDto createSeatDto)
        {
            try
            {
                var seat = await _seatService.CreateAsync(createSeatDto);
                return CreatedAtAction(nameof(GetById), new { id = seat.Id }, seat);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SeatDto>> Update(int id, UpdateSeatDto updateSeatDto)
        {
            try
            {
                var seat = await _seatService.UpdateAsync(id, updateSeatDto);
                return Ok(seat);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deletedBy = "System", [FromQuery] string reason = "Manual deletion")
        {
            try
            {
                await _seatService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
} 