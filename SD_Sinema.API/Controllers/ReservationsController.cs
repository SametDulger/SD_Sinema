using Microsoft.AspNetCore.Mvc;
using SD_Sinema.Business.DTOs;
using SD_Sinema.Business.Services;

namespace SD_Sinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetAll()
        {
            var reservations = await _reservationService.GetAllAsync();
            return Ok(reservations);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetByUser(int userId)
        {
            var reservations = await _reservationService.GetReservationsByUserAsync(userId);
            return Ok(reservations);
        }

        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetBySession(int sessionId)
        {
            var reservations = await _reservationService.GetReservationsBySessionAsync(sessionId);
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetById(int id)
        {
            var reservation = await _reservationService.GetByIdAsync(id);
            if (reservation == null)
                return NotFound();

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Create(CreateReservationDto createReservationDto)
        {
            try
            {
                var reservation = await _reservationService.CreateAsync(createReservationDto);
                return CreatedAtAction(nameof(GetById), new { id = reservation.Id }, reservation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationDto>> Update(int id, UpdateReservationDto updateReservationDto)
        {
            try
            {
                var reservation = await _reservationService.UpdateAsync(id, updateReservationDto);
                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, [FromQuery] string deletedBy, [FromQuery] string reason)
        {
            try
            {
                await _reservationService.DeleteAsync(id, deletedBy, reason);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/approve")]
        public async Task<ActionResult<ReservationDto>> Approve(int id)
        {
            try
            {
                var reservation = await _reservationService.ApproveReservationAsync(id);
                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<ReservationDto>> Cancel(int id)
        {
            try
            {
                var reservation = await _reservationService.CancelReservationAsync(id);
                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
} 